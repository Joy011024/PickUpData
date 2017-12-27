namespace FluorineFx.IO.Bytecode.CodeDom
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using FluorineFx.Util;
    using log4net;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    internal class AMF0ReflectionOptimizer
    {
        private CompilerParameters _cp = new CompilerParameters();
        protected Layouter _layouter;
        protected Type _mappedClass;
        protected AMFReader _reader;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMF0ReflectionOptimizer));

        public AMF0ReflectionOptimizer(Type type, AMFReader reader)
        {
            this._mappedClass = type;
            this._reader = reader;
            this._layouter = new Layouter();
        }

        private void AddAssembly(string name)
        {
            if (!name.StartsWith("System.") && !this._cp.ReferencedAssemblies.Contains(name))
            {
                this._cp.ReferencedAssemblies.Add(name);
            }
        }

        private IReflectionOptimizer Build(string code)
        {
            CompilerResults results;
            CodeDomProvider provider = new CSharpCodeProvider();
            if (FluorineConfiguration.Instance.OptimizerSettings.Debug)
            {
                string path = Path.Combine(Path.GetTempPath(), this._mappedClass.FullName.Replace('.', '_').Replace("+", "__")) + ".cs";
                StreamWriter writer = File.CreateText(path);
                writer.Write(code);
                writer.Close();
                this._cp.TempFiles = new TempFileCollection(Path.GetTempPath());
                this._cp.TempFiles.KeepFiles = true;
                results = provider.CompileAssemblyFromFile(this._cp, new string[] { path });
            }
            else
            {
                results = provider.CompileAssemblyFromSource(this._cp, new string[] { code });
            }
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    log.Error(__Res.GetString("Compiler_Error", new object[] { error.Line, error.Column, error.ErrorText }));
                }
                throw new InvalidOperationException(results.Errors[0].ErrorText);
            }
            Assembly compiledAssembly = results.CompiledAssembly;
            Type[] types = compiledAssembly.GetTypes();
            return (IReflectionOptimizer) compiledAssembly.CreateInstance(types[0].FullName, false, BindingFlags.CreateInstance, null, null, null, null);
        }

        protected bool DoTypeCheck()
        {
            return FluorineConfiguration.Instance.OptimizerSettings.TypeCheck;
        }

        public IReflectionOptimizer Generate(object instance)
        {
            try
            {
                this.InitCompiler();
                return this.Build(this.GenerateCode(instance));
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual string GenerateCode(object instance)
        {
            this._layouter.Append(this.GetHeader());
            this._layouter.AppendFormat(this.GetClassDefinition(), new object[] { this._mappedClass.FullName.Replace('.', '_').Replace("+", "__"), this._mappedClass.FullName });
            this._layouter.Begin();
            this._layouter.Begin();
            this._layouter.AppendFormat("{0} instance = new {0}();", new object[] { this._mappedClass.FullName });
            this._layouter.Append("reader.AddReference(instance);");
            Type type = instance.GetType();
            string memberName = this._reader.ReadString();
            this._layouter.Append("byte typeCode = 0;");
            this._layouter.Append("string key = null;");
            for (byte i = this._reader.ReadByte(); i != 9; i = this._reader.ReadByte())
            {
                this._layouter.Append("key = reader.ReadString();");
                this._layouter.Append("typeCode = reader.ReadByte();");
                object obj2 = this._reader.ReadData(i);
                this._reader.SetMember(instance, memberName, obj2);
                MemberInfo[] member = type.GetMember(memberName);
                if ((member == null) || (member.Length <= 0))
                {
                    throw new MissingMemberException(this._mappedClass.FullName, memberName);
                }
                this.GeneratePropertySet(member[0]);
                memberName = this._reader.ReadString();
            }
            this._layouter.Append("key = reader.ReadString();");
            this._layouter.Append("typeCode = reader.ReadByte();");
            this._layouter.Append("if( typeCode != AMF0TypeCode.EndOfObject ) throw new UnexpectedAMF();");
            this._layouter.Append("return instance;");
            this._layouter.End();
            this._layouter.Append("}");
            this._layouter.End();
            this._layouter.Append("}");
            this._layouter.Append("}");
            return this._layouter.ToString();
        }

        protected void GenerateElseThrowUnexpectedAMFException(MemberInfo memberInfo)
        {
            this._layouter.Append("else");
            this._layouter.Begin();
            this._layouter.AppendFormat("throw new UnexpectedAMF(\"Unexpected data for member {0}\");", new object[] { memberInfo.Name });
            this._layouter.End();
        }

        private void GeneratePropertySet(MemberInfo memberInfo)
        {
            Type propertyType = null;
            if (memberInfo is PropertyInfo)
            {
                PropertyInfo info = memberInfo as PropertyInfo;
                propertyType = info.PropertyType;
            }
            if (memberInfo is FieldInfo)
            {
                FieldInfo info2 = memberInfo as FieldInfo;
                propertyType = info2.FieldType;
            }
            this._layouter.AppendFormat("//Setting member {0}", new object[] { memberInfo.Name });
            if (propertyType.IsPrimitive || (propertyType == typeof(decimal)))
            {
                switch (Type.GetTypeCode(propertyType))
                {
                    case TypeCode.Boolean:
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Append("if( typeCode == AMF0TypeCode.Boolean )");
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Begin();
                        }
                        this._layouter.AppendFormat("instance.{0} = reader.ReadBoolean();", new object[] { memberInfo.Name });
                        if (this.DoTypeCheck())
                        {
                            this._layouter.End();
                        }
                        if (this.DoTypeCheck())
                        {
                            this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                        }
                        break;

                    case TypeCode.Char:
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Append("if( typeCode == AMF0TypeCode.String )");
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Append("{");
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Begin();
                        }
                        this._layouter.AppendFormat("string str{0} = reader.ReadString();", new object[] { memberInfo.Name });
                        if (this.DoTypeCheck())
                        {
                            this._layouter.AppendFormat("if( str{0} != null && str{0} != string.Empty )", new object[] { memberInfo.Name });
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Begin();
                        }
                        this._layouter.AppendFormat("instance.{0} = str{0}[0];", new object[] { memberInfo.Name });
                        if (this.DoTypeCheck())
                        {
                            this._layouter.End();
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.End();
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Append("}");
                        }
                        if (this.DoTypeCheck())
                        {
                            this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                        }
                        break;

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Append("if( typeCode == AMF0TypeCode.Number )");
                        }
                        if (this.DoTypeCheck())
                        {
                            this._layouter.Begin();
                        }
                        this._layouter.AppendFormat("instance.{0} = ({1})reader.ReadDouble();", new object[] { memberInfo.Name, propertyType.FullName });
                        if (this.DoTypeCheck())
                        {
                            this._layouter.End();
                        }
                        if (this.DoTypeCheck())
                        {
                            this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                        }
                        break;
                }
            }
            else if (propertyType.IsEnum)
            {
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("if( typeCode == AMF0TypeCode.String || typeCode == AMF0TypeCode.Number )");
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("{");
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Begin();
                }
                this._layouter.Append("if( typeCode == AMF0TypeCode.String )");
                this._layouter.Begin();
                this._layouter.AppendFormat("instance.{0} = ({1})Enum.Parse(typeof({1}), reader.ReadString(), true);", new object[] { memberInfo.Name, propertyType.FullName });
                this._layouter.End();
                this._layouter.Append("if( typeCode == AMF0TypeCode.Number )");
                this._layouter.Begin();
                this._layouter.AppendFormat("instance.{0} = ({1})Enum.ToObject(typeof({1}), Convert.ToInt32(reader.ReadDouble()));", new object[] { memberInfo.Name, propertyType.FullName });
                this._layouter.End();
                if (this.DoTypeCheck())
                {
                    this._layouter.End();
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("}");
                }
                if (this.DoTypeCheck())
                {
                    this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                }
            }
            else if (propertyType == typeof(DateTime))
            {
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("if( typeCode == AMF0TypeCode.DateTime )");
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Begin();
                }
                this._layouter.AppendFormat("instance.{0} = reader.ReadDateTime();", new object[] { memberInfo.Name });
                if (this.DoTypeCheck())
                {
                    this._layouter.End();
                }
                if (this.DoTypeCheck())
                {
                    this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                }
            }
            else if (propertyType == typeof(Guid))
            {
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("if( typeCode == AMF0TypeCode.String )");
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("{");
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Begin();
                }
                this._layouter.AppendFormat("instance.{0} = new Guid(reader.ReadString());", new object[] { memberInfo.Name });
                if (this.DoTypeCheck())
                {
                    this._layouter.End();
                }
                if (this.DoTypeCheck())
                {
                    this._layouter.Append("}");
                }
                if (this.DoTypeCheck())
                {
                    this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                }
            }
            else
            {
                if (propertyType.IsValueType)
                {
                    throw new FluorineException("Struct value types are not supported");
                }
                if (propertyType == typeof(string))
                {
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Append("if( typeCode == AMF0TypeCode.String || typeCode == AMF0TypeCode.LongString || typeCode == AMF0TypeCode.Null || typeCode == AMF0TypeCode.Undefined )");
                    }
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Append("{");
                    }
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Begin();
                    }
                    this._layouter.Append("if( typeCode == AMF0TypeCode.String )");
                    this._layouter.Begin();
                    this._layouter.AppendFormat("instance.{0} = reader.ReadString();", new object[] { memberInfo.Name });
                    this._layouter.End();
                    this._layouter.Append("if( typeCode == AMF0TypeCode.LongString )");
                    this._layouter.Begin();
                    this._layouter.AppendFormat("instance.{0} = reader.ReadLongString();", new object[] { memberInfo.Name });
                    this._layouter.End();
                    this._layouter.Append("if( typeCode == AMF0TypeCode.Null || typeCode == AMF0TypeCode.Undefined )");
                    this._layouter.Begin();
                    this._layouter.AppendFormat("instance.{0} = null;", new object[] { memberInfo.Name });
                    this._layouter.End();
                    if (this.DoTypeCheck())
                    {
                        this._layouter.End();
                    }
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Append("}");
                    }
                    if (this.DoTypeCheck())
                    {
                        this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                    }
                }
                else if (propertyType == typeof(XmlDocument))
                {
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Append("if( typeCode == AMF0TypeCode.Xml || typeCode == AMF0TypeCode.Null || typeCode == AMF0TypeCode.Undefined )");
                    }
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Append("{");
                    }
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Begin();
                    }
                    this._layouter.Append("if( typeCode == AMF0TypeCode.Xml )");
                    this._layouter.Append("{");
                    this._layouter.Begin();
                    this._layouter.Append("string xml = reader.ReadLongString();");
                    this._layouter.Append("System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();");
                    this._layouter.Append("xmlDocument.LoadXml(xml);");
                    this._layouter.AppendFormat("instance.{0} = xmlDocument;", new object[] { memberInfo.Name });
                    this._layouter.End();
                    this._layouter.Append("}");
                    this._layouter.Append("if( typeCode == AMF0TypeCode.Null || typeCode == AMF0TypeCode.Undefined )");
                    this._layouter.Begin();
                    this._layouter.AppendFormat("instance.{0} = null;", new object[] { memberInfo.Name });
                    this._layouter.End();
                    if (this.DoTypeCheck())
                    {
                        this._layouter.End();
                    }
                    if (this.DoTypeCheck())
                    {
                        this._layouter.Append("}");
                    }
                    if (this.DoTypeCheck())
                    {
                        this.GenerateElseThrowUnexpectedAMFException(memberInfo);
                    }
                }
                else
                {
                    this._layouter.AppendFormat("instance.{0} = ({1})TypeHelper.ChangeType(reader.ReadData(typeCode), typeof({1}));", new object[] { memberInfo.Name, TypeHelper.GetCSharpName(propertyType) });
                }
            }
        }

        protected virtual string GetClassDefinition()
        {
            return "public class {0} : IReflectionOptimizer {{\r\n\t\t\t\t\t\r\n\tpublic {0}() {{\r\n\t}}\r\n\r\n\tpublic object CreateInstance() {{\r\n\t\treturn new {1}();\r\n\t}}\r\n\r\n\tpublic object ReadData(AMFReader reader, ClassDefinition classDefinition) {{\r\n\t";
        }

        protected virtual string GetHeader()
        {
            return "using System;\nusing System.Reflection;\nusing FluorineFx;\nusing FluorineFx.AMF3;\nusing FluorineFx.IO;\nusing FluorineFx.Exceptions;\nusing FluorineFx.Configuration;\nusing FluorineFx.IO.Bytecode;\nusing log4net;\nnamespace FluorineFx.Bytecode.CodeDom {\n";
        }

        private void InitCompiler()
        {
            Assembly assembly2;
            this._cp.GenerateInMemory = true;
            if (FluorineConfiguration.Instance.OptimizerSettings.Debug)
            {
                this._cp.GenerateInMemory = false;
                this._cp.IncludeDebugInformation = true;
            }
            this._cp.TreatWarningsAsErrors = false;
            this.AddAssembly(Assembly.GetExecutingAssembly().Location);
            Assembly assembly = this._mappedClass.Assembly;
            this.AddAssembly(assembly.Location);
            foreach (AssemblyName name in assembly.GetReferencedAssemblies())
            {
                assembly2 = Assembly.Load(name);
                this.AddAssembly(assembly2.Location);
            }
            foreach (AssemblyName name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                assembly2 = Assembly.Load(name);
                this.AddAssembly(assembly2.Location);
            }
        }
    }
}

