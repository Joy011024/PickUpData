namespace FluorineFx.IO.Bytecode.CodeDom
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using log4net;
    using System;
    using System.Reflection;
    using System.Xml;

    internal class AMF3ReflectionOptimizer : AMF0ReflectionOptimizer
    {
        private ClassDefinition _classDefinition;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMF3ReflectionOptimizer));

        public AMF3ReflectionOptimizer(Type type, ClassDefinition classDefinition, AMFReader reader) : base(type, reader)
        {
            this._classDefinition = classDefinition;
        }

        protected override string GenerateCode(object instance)
        {
            Type type = instance.GetType();
            base._layouter.Append(this.GetHeader());
            base._layouter.AppendFormat(this.GetClassDefinition(), new object[] { base._mappedClass.FullName.Replace('.', '_').Replace("+", "__"), base._mappedClass.FullName });
            base._layouter.Begin();
            base._layouter.Begin();
            base._layouter.AppendFormat("{0} instance = new {0}();", new object[] { base._mappedClass.FullName });
            base._layouter.Append("reader.AddAMF3ObjectReference(instance);");
            base._layouter.Append("byte typeCode = 0;");
            for (int i = 0; i < this._classDefinition.MemberCount; i++)
            {
                string name = this._classDefinition.Members[i].Name;
                object obj2 = base._reader.ReadAMF3Data();
                base._reader.SetMember(instance, name, obj2);
                base._layouter.Append("typeCode = reader.ReadByte();");
                MemberInfo[] member = type.GetMember(name);
                if ((member == null) || (member.Length <= 0))
                {
                    throw new MissingMemberException(base._mappedClass.FullName, name);
                }
                this.GeneratePropertySet(member[0]);
            }
            base._layouter.Append("return instance;");
            base._layouter.End();
            base._layouter.Append("}");
            base._layouter.End();
            base._layouter.Append("}");
            base._layouter.Append("}");
            return base._layouter.ToString();
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
            base._layouter.AppendFormat("//Setting member {0}", new object[] { memberInfo.Name });
            if (propertyType.IsPrimitive || (propertyType == typeof(decimal)))
            {
                switch (Type.GetTypeCode(propertyType))
                {
                    case TypeCode.Boolean:
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("if( typeCode == AMF3TypeCode.BooleanFalse || typeCode == AMF3TypeCode.BooleanTrue )");
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("{");
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Begin();
                        }
                        base._layouter.Append("if( typeCode == AMF3TypeCode.BooleanFalse )");
                        base._layouter.Begin();
                        base._layouter.AppendFormat("instance.{0} = false;", new object[] { memberInfo.Name });
                        base._layouter.End();
                        base._layouter.Append("if( typeCode == AMF3TypeCode.BooleanTrue )");
                        base._layouter.Begin();
                        base._layouter.AppendFormat("instance.{0} = true;", new object[] { memberInfo.Name });
                        base._layouter.End();
                        if (base.DoTypeCheck())
                        {
                            base._layouter.End();
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("}");
                        }
                        if (base.DoTypeCheck())
                        {
                            base.GenerateElseThrowUnexpectedAMFException(memberInfo);
                        }
                        break;

                    case TypeCode.Char:
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("if( typeCode == AMF3TypeCode.String )");
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("{");
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Begin();
                        }
                        base._layouter.AppendFormat("string str{0} = reader.ReadAMF3String();", new object[] { memberInfo.Name });
                        if (base.DoTypeCheck())
                        {
                            base._layouter.AppendFormat("if( str{0} != null && str{0} != string.Empty )", new object[] { memberInfo.Name });
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Begin();
                        }
                        base._layouter.AppendFormat("instance.{0} = str{0}[0];", new object[] { memberInfo.Name });
                        if (base.DoTypeCheck())
                        {
                            base._layouter.End();
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.End();
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("}");
                        }
                        if (base.DoTypeCheck())
                        {
                            base.GenerateElseThrowUnexpectedAMFException(memberInfo);
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
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("if( typeCode == AMF3TypeCode.Number || typeCode == AMF3TypeCode.Integer )");
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("{");
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Begin();
                        }
                        base._layouter.Append("if( typeCode == AMF3TypeCode.Number )");
                        base._layouter.Begin();
                        base._layouter.AppendFormat("instance.{0} = ({1})reader.ReadDouble();", new object[] { memberInfo.Name, propertyType.FullName });
                        base._layouter.End();
                        base._layouter.Append("if( typeCode == AMF3TypeCode.Integer )");
                        base._layouter.Begin();
                        base._layouter.AppendFormat("instance.{0} = ({1})(int)reader.ReadAMF3Int();", new object[] { memberInfo.Name, propertyType.FullName });
                        base._layouter.End();
                        if (base.DoTypeCheck())
                        {
                            base._layouter.End();
                        }
                        if (base.DoTypeCheck())
                        {
                            base._layouter.Append("}");
                        }
                        if (base.DoTypeCheck())
                        {
                            base.GenerateElseThrowUnexpectedAMFException(memberInfo);
                        }
                        break;
                }
            }
            else if (propertyType.IsEnum)
            {
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("if( typeCode == AMF3TypeCode.String || typeCode == AMF3TypeCode.Integer )");
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("{");
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Begin();
                }
                base._layouter.Append("if( typeCode == AMF3TypeCode.String )");
                base._layouter.Begin();
                base._layouter.AppendFormat("instance.{0} = ({1})Enum.Parse(typeof({1}), reader.ReadAMF3String(), true);", new object[] { memberInfo.Name, propertyType.FullName });
                base._layouter.End();
                base._layouter.Append("if( typeCode == AMF3TypeCode.Integer )");
                base._layouter.Begin();
                base._layouter.AppendFormat("instance.{0} = ({1})Enum.ToObject(typeof({1}), reader.ReadAMF3Int());", new object[] { memberInfo.Name, propertyType.FullName });
                base._layouter.End();
                if (base.DoTypeCheck())
                {
                    base._layouter.End();
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("}");
                }
                if (base.DoTypeCheck())
                {
                    base.GenerateElseThrowUnexpectedAMFException(memberInfo);
                }
            }
            else if (propertyType == typeof(DateTime))
            {
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("if( typeCode == AMF3TypeCode.DateTime )");
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Begin();
                }
                base._layouter.AppendFormat("instance.{0} = reader.ReadAMF3Date();", new object[] { memberInfo.Name });
                if (base.DoTypeCheck())
                {
                    base._layouter.End();
                }
                if (base.DoTypeCheck())
                {
                    base.GenerateElseThrowUnexpectedAMFException(memberInfo);
                }
            }
            else if (propertyType == typeof(Guid))
            {
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("if( typeCode == AMF3TypeCode.String )");
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("{");
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Begin();
                }
                base._layouter.AppendFormat("instance.{0} = new Guid(reader.ReadAMF3String());", new object[] { memberInfo.Name });
                if (base.DoTypeCheck())
                {
                    base._layouter.End();
                }
                if (base.DoTypeCheck())
                {
                    base._layouter.Append("}");
                }
                if (base.DoTypeCheck())
                {
                    base.GenerateElseThrowUnexpectedAMFException(memberInfo);
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
                    if (base.DoTypeCheck())
                    {
                        base._layouter.Append("if( typeCode == AMF3TypeCode.String || typeCode == AMF3TypeCode.Null || typeCode == AMF3TypeCode.Undefined )");
                    }
                    if (base.DoTypeCheck())
                    {
                        base._layouter.Append("{");
                    }
                    if (base.DoTypeCheck())
                    {
                        base._layouter.Begin();
                    }
                    base._layouter.Append("if( typeCode == AMF3TypeCode.String )");
                    base._layouter.Begin();
                    base._layouter.AppendFormat("instance.{0} = reader.ReadAMF3String();", new object[] { memberInfo.Name });
                    base._layouter.End();
                    base._layouter.Append("if( typeCode == AMF3TypeCode.Null || typeCode == AMF3TypeCode.Undefined )");
                    base._layouter.Begin();
                    base._layouter.AppendFormat("instance.{0} = null;", new object[] { memberInfo.Name });
                    base._layouter.End();
                    if (base.DoTypeCheck())
                    {
                        base._layouter.End();
                    }
                    if (base.DoTypeCheck())
                    {
                        base._layouter.Append("}");
                    }
                    if (base.DoTypeCheck())
                    {
                        base.GenerateElseThrowUnexpectedAMFException(memberInfo);
                    }
                }
                else if (propertyType == typeof(XmlDocument))
                {
                    base._layouter.Append("if( typeCode == AMF3TypeCode.Xml || typeCode == AMF3TypeCode.Xml2 || typeCode == AMF3TypeCode.Null || typeCode == AMF3TypeCode.Undefined )");
                    base._layouter.Append("{");
                    base._layouter.Begin();
                    base._layouter.Append("if( typeCode == AMF3TypeCode.Xml || typeCode == AMF3TypeCode.Xml2 )");
                    base._layouter.Begin();
                    base._layouter.AppendFormat("instance.{0} = reader.ReadAMF3XmlDocument();", new object[] { memberInfo.Name });
                    base._layouter.End();
                    base._layouter.Append("if( typeCode == AMF3TypeCode.Null || typeCode == AMF3TypeCode.Undefined )");
                    base._layouter.Begin();
                    base._layouter.AppendFormat("instance.{0} = null;", new object[] { memberInfo.Name });
                    base._layouter.End();
                    base._layouter.End();
                    base._layouter.Append("}");
                    base.GenerateElseThrowUnexpectedAMFException(memberInfo);
                }
                else
                {
                    base._layouter.AppendFormat("instance.{0} = ({1})TypeHelper.ChangeType(reader.ReadAMF3Data(typeCode), typeof({1}));", new object[] { memberInfo.Name, TypeHelper.GetCSharpName(propertyType) });
                }
            }
        }
    }
}

