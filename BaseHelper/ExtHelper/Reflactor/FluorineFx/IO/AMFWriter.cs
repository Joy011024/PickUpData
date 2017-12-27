namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.Configuration;
    using FluorineFx.Exceptions;
    using FluorineFx.IO.Writers;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public class AMFWriter : BinaryWriter
    {
        private Dictionary<object, int> _amf0ObjectReferences;
        private Dictionary<ClassDefinition, int> _classDefinitionReferences;
        private Dictionary<object, int> _objectReferences;
        private Dictionary<object, int> _stringReferences;
        private bool _useLegacyCollection;
        private static Dictionary<Type, IAMFWriter>[] AmfWriterTable;
        private static Dictionary<string, ClassDefinition> classDefinitions;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMFWriter));

        static AMFWriter()
        {
            Dictionary<Type, IAMFWriter> dictionary = new Dictionary<Type, IAMFWriter>();
            AMF0NumberWriter writer = new AMF0NumberWriter();
            dictionary.Add(typeof(sbyte), writer);
            dictionary.Add(typeof(byte), writer);
            dictionary.Add(typeof(short), writer);
            dictionary.Add(typeof(ushort), writer);
            dictionary.Add(typeof(int), writer);
            dictionary.Add(typeof(uint), writer);
            dictionary.Add(typeof(long), writer);
            dictionary.Add(typeof(ulong), writer);
            dictionary.Add(typeof(float), writer);
            dictionary.Add(typeof(double), writer);
            dictionary.Add(typeof(decimal), writer);
            dictionary.Add(typeof(DBNull), new AMF0NullWriter());
            AMF0SqlTypesWriter writer2 = new AMF0SqlTypesWriter();
            dictionary.Add(typeof(INullable), writer2);
            dictionary.Add(typeof(SqlByte), writer2);
            dictionary.Add(typeof(SqlInt16), writer2);
            dictionary.Add(typeof(SqlInt32), writer2);
            dictionary.Add(typeof(SqlInt64), writer2);
            dictionary.Add(typeof(SqlSingle), writer2);
            dictionary.Add(typeof(SqlDouble), writer2);
            dictionary.Add(typeof(SqlDecimal), writer2);
            dictionary.Add(typeof(SqlMoney), writer2);
            dictionary.Add(typeof(SqlDateTime), writer2);
            dictionary.Add(typeof(SqlString), writer2);
            dictionary.Add(typeof(SqlGuid), writer2);
            dictionary.Add(typeof(SqlBinary), writer2);
            dictionary.Add(typeof(SqlBoolean), writer2);
            dictionary.Add(typeof(CacheableObject), new AMF0CacheableObjectWriter());
            dictionary.Add(typeof(XmlDocument), new AMF0XmlDocumentWriter());
            dictionary.Add(typeof(DataTable), new AMF0DataTableWriter());
            dictionary.Add(typeof(DataSet), new AMF0DataSetWriter());
            dictionary.Add(typeof(RawBinary), new RawBinaryWriter());
            dictionary.Add(typeof(NameObjectCollectionBase), new AMF0NameObjectCollectionWriter());
            dictionary.Add(typeof(XDocument), new AMF0XDocumentWriter());
            dictionary.Add(typeof(XElement), new AMF0XElementWriter());
            dictionary.Add(typeof(Guid), new AMF0GuidWriter());
            dictionary.Add(typeof(string), new AMF0StringWriter());
            dictionary.Add(typeof(bool), new AMF0BooleanWriter());
            dictionary.Add(typeof(Enum), new AMF0EnumWriter());
            dictionary.Add(typeof(char), new AMF0CharWriter());
            dictionary.Add(typeof(DateTime), new AMF0DateTimeWriter());
            dictionary.Add(typeof(Array), new AMF0ArrayWriter());
            dictionary.Add(typeof(ASObject), new AMF0ASObjectWriter());
            Dictionary<Type, IAMFWriter> dictionary2 = new Dictionary<Type, IAMFWriter>();
            AMF3IntWriter writer3 = new AMF3IntWriter();
            AMF3DoubleWriter writer4 = new AMF3DoubleWriter();
            dictionary2.Add(typeof(sbyte), writer3);
            dictionary2.Add(typeof(byte), writer3);
            dictionary2.Add(typeof(short), writer3);
            dictionary2.Add(typeof(ushort), writer3);
            dictionary2.Add(typeof(int), writer3);
            dictionary2.Add(typeof(uint), writer3);
            dictionary2.Add(typeof(long), writer4);
            dictionary2.Add(typeof(ulong), writer4);
            dictionary2.Add(typeof(float), writer4);
            dictionary2.Add(typeof(double), writer4);
            dictionary2.Add(typeof(decimal), writer4);
            dictionary2.Add(typeof(DBNull), new AMF3DBNullWriter());
            AMF3SqlTypesWriter writer5 = new AMF3SqlTypesWriter();
            dictionary2.Add(typeof(INullable), writer5);
            dictionary2.Add(typeof(SqlByte), writer5);
            dictionary2.Add(typeof(SqlInt16), writer5);
            dictionary2.Add(typeof(SqlInt32), writer5);
            dictionary2.Add(typeof(SqlInt64), writer5);
            dictionary2.Add(typeof(SqlSingle), writer5);
            dictionary2.Add(typeof(SqlDouble), writer5);
            dictionary2.Add(typeof(SqlDecimal), writer5);
            dictionary2.Add(typeof(SqlMoney), writer5);
            dictionary2.Add(typeof(SqlDateTime), writer5);
            dictionary2.Add(typeof(SqlString), writer5);
            dictionary2.Add(typeof(SqlGuid), writer5);
            dictionary2.Add(typeof(SqlBinary), writer5);
            dictionary2.Add(typeof(SqlBoolean), writer5);
            dictionary2.Add(typeof(CacheableObject), new AMF3CacheableObjectWriter());
            dictionary2.Add(typeof(XmlDocument), new AMF3XmlDocumentWriter());
            dictionary2.Add(typeof(DataTable), new AMF3DataTableWriter());
            dictionary2.Add(typeof(DataSet), new AMF3DataSetWriter());
            dictionary2.Add(typeof(RawBinary), new RawBinaryWriter());
            dictionary2.Add(typeof(NameObjectCollectionBase), new AMF3NameObjectCollectionWriter());
            dictionary2.Add(typeof(XDocument), new AMF3XDocumentWriter());
            dictionary2.Add(typeof(XElement), new AMF3XElementWriter());
            dictionary2.Add(typeof(Guid), new AMF3GuidWriter());
            dictionary2.Add(typeof(string), new AMF3StringWriter());
            dictionary2.Add(typeof(bool), new AMF3BooleanWriter());
            dictionary2.Add(typeof(Enum), new AMF3EnumWriter());
            dictionary2.Add(typeof(char), new AMF3CharWriter());
            dictionary2.Add(typeof(DateTime), new AMF3DateTimeWriter());
            dictionary2.Add(typeof(Array), new AMF3ArrayWriter());
            dictionary2.Add(typeof(ASObject), new AMF3ASObjectWriter());
            dictionary2.Add(typeof(ByteArray), new AMF3ByteArrayWriter());
            Dictionary<Type, IAMFWriter>[] dictionaryArray = new Dictionary<Type, IAMFWriter>[4];
            dictionaryArray[0] = dictionary;
            dictionaryArray[3] = dictionary2;
            AmfWriterTable = dictionaryArray;
            classDefinitions = new Dictionary<string, ClassDefinition>();
        }

        public AMFWriter(Stream stream) : base(stream)
        {
            this._useLegacyCollection = true;
            this.Reset();
        }

        internal AMFWriter(AMFWriter writer, Stream stream) : base(stream)
        {
            this._useLegacyCollection = true;
            this._amf0ObjectReferences = writer._amf0ObjectReferences;
            this._objectReferences = writer._objectReferences;
            this._stringReferences = writer._stringReferences;
            this._classDefinitionReferences = writer._classDefinitionReferences;
            this._useLegacyCollection = writer._useLegacyCollection;
        }

        internal void AddReference(object value)
        {
            this._amf0ObjectReferences.Add(value, this._amf0ObjectReferences.Count);
        }

        private ClassDefinition CreateClassDefinition(object obj)
        {
            ClassDefinition definition = null;
            ClassMember member;
            Type type = obj.GetType();
            bool externalizable = type.GetInterface(typeof(IExternalizable).FullName, true) != null;
            bool dynamic = false;
            string className = null;
            if (obj is IDictionary)
            {
                if ((obj is ASObject) && (obj as ASObject).IsTypedObject)
                {
                    ASObject obj2 = obj as ASObject;
                    ClassMember[] memberArray = new ClassMember[obj2.Count];
                    int index = 0;
                    foreach (KeyValuePair<string, object> pair in obj2)
                    {
                        member = new ClassMember(pair.Key, BindingFlags.Default, MemberTypes.Custom);
                        memberArray[index] = member;
                        index++;
                    }
                    className = obj2.TypeName;
                    definition = new ClassDefinition(className, memberArray, externalizable, dynamic);
                    classDefinitions[className] = definition;
                    return definition;
                }
                dynamic = true;
                return new ClassDefinition(string.Empty, ClassDefinition.EmptyClassMembers, externalizable, dynamic);
            }
            if (obj is IExternalizable)
            {
                className = type.FullName;
                definition = new ClassDefinition(FluorineConfiguration.Instance.GetCustomClass(className), ClassDefinition.EmptyClassMembers, true, false);
                classDefinitions[type.FullName] = definition;
                return definition;
            }
            List<string> list = new List<string>();
            List<ClassMember> list2 = new List<ClassMember>();
            foreach (PropertyInfo info in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string name = info.Name;
                if (info.GetCustomAttributes(typeof(TransientAttribute), true).Length <= 0)
                {
                    if ((info.GetGetMethod() == null) || (info.GetGetMethod().GetParameters().Length > 0))
                    {
                        string str3 = __Res.GetString("Reflection_PropertyIndexFail", new object[] { string.Format("{0}.{1}", type.FullName, info.Name) });
                        if (log.get_IsWarnEnabled())
                        {
                            log.Warn(str3);
                        }
                    }
                    else if (!list.Contains(name))
                    {
                        list.Add(name);
                        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
                        try
                        {
                            PropertyInfo property = obj.GetType().GetProperty(name);
                        }
                        catch (AmbiguousMatchException)
                        {
                            bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                        }
                        member = new ClassMember(name, bindingFlags, info.MemberType);
                        list2.Add(member);
                    }
                }
            }
            foreach (FieldInfo info3 in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if ((info3.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length <= 0) && (info3.GetCustomAttributes(typeof(TransientAttribute), true).Length <= 0))
                {
                    member = new ClassMember(info3.Name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, info3.MemberType);
                    list2.Add(member);
                }
            }
            ClassMember[] members = list2.ToArray();
            className = type.FullName;
            definition = new ClassDefinition(FluorineConfiguration.Instance.GetCustomClass(className), members, externalizable, dynamic);
            classDefinitions[type.FullName] = definition;
            return definition;
        }

        private ClassDefinition GetClassDefinition(object obj)
        {
            if (obj is ASObject)
            {
                ASObject obj2 = obj as ASObject;
                if (obj2.IsTypedObject && classDefinitions.ContainsKey(obj2.TypeName))
                {
                    return classDefinitions[obj2.TypeName];
                }
                return null;
            }
            if (classDefinitions.ContainsKey(obj.GetType().FullName))
            {
                return classDefinitions[obj.GetType().FullName];
            }
            return null;
        }

        internal object GetMember(object instance, ClassMember member)
        {
            if (instance is ASObject)
            {
                ASObject obj2 = instance as ASObject;
                if (obj2.ContainsKey(member.Name))
                {
                    return obj2[member.Name];
                }
            }
            Type type = instance.GetType();
            if (member.MemberType == MemberTypes.Property)
            {
                return type.GetProperty(member.Name, member.BindingFlags).GetValue(instance, null);
            }
            if (member.MemberType != MemberTypes.Field)
            {
                throw new FluorineException(__Res.GetString("Reflection_MemberNotFound", new object[] { string.Format("{0}.{1}", type.FullName, member.Name) }));
            }
            return type.GetField(member.Name, member.BindingFlags).GetValue(instance);
        }

        public void Reset()
        {
            this._amf0ObjectReferences = new Dictionary<object, int>(5);
            this._objectReferences = new Dictionary<object, int>(5);
            this._stringReferences = new Dictionary<object, int>(5);
            this._classDefinitionReferences = new Dictionary<ClassDefinition, int>();
        }

        public void WriteAMF3Array(Array value)
        {
            if (this._amf0ObjectReferences.ContainsKey(value))
            {
                this.WriteReference(value);
            }
            else
            {
                int num;
                if (!this._objectReferences.ContainsKey(value))
                {
                    this._objectReferences.Add(value, this._objectReferences.Count);
                    num = value.Length << 1;
                    num |= 1;
                    this.WriteAMF3IntegerData(num);
                    this.WriteAMF3UTF(string.Empty);
                    for (int i = 0; i < value.Length; i++)
                    {
                        this.WriteAMF3Data(value.GetValue(i));
                    }
                }
                else
                {
                    num = this._objectReferences[value];
                    num = num << 1;
                    this.WriteAMF3IntegerData(num);
                }
            }
        }

        public void WriteAMF3Array(IList value)
        {
            int num;
            if (!this._objectReferences.ContainsKey(value))
            {
                this._objectReferences.Add(value, this._objectReferences.Count);
                num = value.Count << 1;
                num |= 1;
                this.WriteAMF3IntegerData(num);
                this.WriteAMF3UTF(string.Empty);
                for (int i = 0; i < value.Count; i++)
                {
                    this.WriteAMF3Data(value[i]);
                }
            }
            else
            {
                num = this._objectReferences[value];
                num = num << 1;
                this.WriteAMF3IntegerData(num);
            }
        }

        public void WriteAMF3AssociativeArray(IDictionary value)
        {
            if (!this._objectReferences.ContainsKey(value))
            {
                this._objectReferences.Add(value, this._objectReferences.Count);
                this.WriteAMF3IntegerData(1);
                foreach (DictionaryEntry entry in value)
                {
                    this.WriteAMF3UTF(entry.Key.ToString());
                    this.WriteAMF3Data(entry.Value);
                }
                this.WriteAMF3UTF(string.Empty);
            }
            else
            {
                int num = this._objectReferences[value];
                num = num << 1;
                this.WriteAMF3IntegerData(num);
            }
        }

        public void WriteAMF3Bool(bool value)
        {
            this.WriteByte(value ? ((byte) 3) : ((byte) 2));
        }

        public void WriteAMF3Data(object data)
        {
            if (data == null)
            {
                this.WriteAMF3Null();
            }
            else if (data is DBNull)
            {
                this.WriteAMF3Null();
            }
            else
            {
                if (FluorineConfiguration.Instance.AcceptNullValueTypes && (FluorineConfiguration.Instance.NullableValues != null))
                {
                    Type type = data.GetType();
                    if (FluorineConfiguration.Instance.NullableValues.ContainsKey(type) && data.Equals(FluorineConfiguration.Instance.NullableValues[type]))
                    {
                        this.WriteAMF3Null();
                        return;
                    }
                }
                IAMFWriter writer = null;
                if (AmfWriterTable[3].ContainsKey(data.GetType()))
                {
                    writer = AmfWriterTable[3][data.GetType()];
                }
                if ((writer == null) && AmfWriterTable[3].ContainsKey(data.GetType().BaseType))
                {
                    writer = AmfWriterTable[3][data.GetType().BaseType];
                }
                if (writer == null)
                {
                    lock (AmfWriterTable)
                    {
                        if (!AmfWriterTable[3].ContainsKey(data.GetType()))
                        {
                            writer = new AMF3ObjectWriter();
                            AmfWriterTable[3].Add(data.GetType(), writer);
                        }
                        else
                        {
                            writer = AmfWriterTable[3][data.GetType()];
                        }
                    }
                }
                if (writer == null)
                {
                    throw new FluorineException(string.Format("Could not find serializer for type {0}", data.GetType().FullName));
                }
                writer.WriteData(this, data);
            }
        }

        public void WriteAMF3DateTime(DateTime value)
        {
            int num;
            if (!this._objectReferences.ContainsKey(value))
            {
                this._objectReferences.Add(value, this._objectReferences.Count);
                num = 1;
                this.WriteAMF3IntegerData(num);
                DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                value = value.ToUniversalTime();
                long totalMilliseconds = (long) value.Subtract(time).TotalMilliseconds;
                this.WriteDouble((double) totalMilliseconds);
            }
            else
            {
                num = this._objectReferences[value];
                num = num << 1;
                this.WriteAMF3IntegerData(num);
            }
        }

        public void WriteAMF3Double(double value)
        {
            this.WriteByte(5);
            this.WriteDouble(value);
        }

        public void WriteAMF3Int(int value)
        {
            if ((value >= -268435456) && (value <= 0xfffffff))
            {
                this.WriteByte(4);
                this.WriteAMF3IntegerData(value);
            }
            else
            {
                this.WriteAMF3Double((double) value);
            }
        }

        private void WriteAMF3IntegerData(int value)
        {
            value &= 0x1fffffff;
            if (value < 0x80)
            {
                this.WriteByte((byte) value);
            }
            else if (value < 0x4000)
            {
                this.WriteByte((byte) (((value >> 7) & 0x7f) | 0x80));
                this.WriteByte((byte) (value & 0x7f));
            }
            else if (value < 0x200000)
            {
                this.WriteByte((byte) (((value >> 14) & 0x7f) | 0x80));
                this.WriteByte((byte) (((value >> 7) & 0x7f) | 0x80));
                this.WriteByte((byte) (value & 0x7f));
            }
            else
            {
                this.WriteByte((byte) (((value >> 0x16) & 0x7f) | 0x80));
                this.WriteByte((byte) (((value >> 15) & 0x7f) | 0x80));
                this.WriteByte((byte) (((value >> 8) & 0x7f) | 0x80));
                this.WriteByte((byte) (value & 0xff));
            }
        }

        public void WriteAMF3Null()
        {
            this.WriteByte(1);
        }

        public void WriteAMF3Object(object value)
        {
            int num;
            if (!this._objectReferences.ContainsKey(value))
            {
                int num2;
                this._objectReferences.Add(value, this._objectReferences.Count);
                ClassDefinition classDefinition = this.GetClassDefinition(value);
                if ((classDefinition != null) && this._classDefinitionReferences.ContainsKey(classDefinition))
                {
                    num = this._classDefinitionReferences[classDefinition];
                    num = num << 2;
                    num |= 1;
                    this.WriteAMF3IntegerData(num);
                }
                else
                {
                    classDefinition = this.CreateClassDefinition(value);
                    this._classDefinitionReferences.Add(classDefinition, this._classDefinitionReferences.Count);
                    num = classDefinition.MemberCount << 1;
                    num |= classDefinition.IsDynamic ? 1 : 0;
                    num = num << 1;
                    num |= classDefinition.IsExternalizable ? 1 : 0;
                    num = num << 2;
                    num |= 3;
                    this.WriteAMF3IntegerData(num);
                    this.WriteAMF3UTF(classDefinition.ClassName);
                    for (num2 = 0; num2 < classDefinition.MemberCount; num2++)
                    {
                        string name = classDefinition.Members[num2].Name;
                        this.WriteAMF3UTF(name);
                    }
                }
                if (classDefinition.IsExternalizable)
                {
                    if (!(value is IExternalizable))
                    {
                        throw new FluorineException(__Res.GetString("Externalizable_CastFail", new object[] { classDefinition.ClassName }));
                    }
                    IExternalizable externalizable = value as IExternalizable;
                    DataOutput output = new DataOutput(this);
                    externalizable.WriteExternal(output);
                }
                else
                {
                    for (num2 = 0; num2 < classDefinition.MemberCount; num2++)
                    {
                        object member = this.GetMember(value, classDefinition.Members[num2]);
                        this.WriteAMF3Data(member);
                    }
                    if (classDefinition.IsDynamic)
                    {
                        IDictionary dictionary = value as IDictionary;
                        foreach (DictionaryEntry entry in dictionary)
                        {
                            this.WriteAMF3UTF(entry.Key.ToString());
                            this.WriteAMF3Data(entry.Value);
                        }
                        this.WriteAMF3UTF(string.Empty);
                    }
                }
            }
            else
            {
                num = this._objectReferences[value];
                num = num << 1;
                this.WriteAMF3IntegerData(num);
            }
        }

        public void WriteAMF3String(string value)
        {
            this.WriteByte(6);
            this.WriteAMF3UTF(value);
        }

        public void WriteAMF3UTF(string value)
        {
            if (value == string.Empty)
            {
                this.WriteAMF3IntegerData(1);
            }
            else
            {
                int num2;
                if (!this._stringReferences.ContainsKey(value))
                {
                    this._stringReferences.Add(value, this._stringReferences.Count);
                    UTF8Encoding encoding = new UTF8Encoding();
                    num2 = encoding.GetByteCount(value) << 1;
                    num2 |= 1;
                    this.WriteAMF3IntegerData(num2);
                    byte[] bytes = encoding.GetBytes(value);
                    if (bytes.Length > 0)
                    {
                        this.Write(bytes);
                    }
                }
                else
                {
                    num2 = this._stringReferences[value];
                    num2 = num2 << 1;
                    this.WriteAMF3IntegerData(num2);
                }
            }
        }

        public void WriteAMF3XDocument(XDocument xDocument)
        {
            this.WriteByte(11);
            string key = string.Empty;
            if (xDocument != null)
            {
                key = xDocument.ToString();
            }
            if (key == string.Empty)
            {
                this.WriteAMF3IntegerData(1);
            }
            else
            {
                int num2;
                if (!this._objectReferences.ContainsKey(key))
                {
                    this._objectReferences.Add(key, this._objectReferences.Count);
                    UTF8Encoding encoding = new UTF8Encoding();
                    num2 = encoding.GetByteCount(key) << 1;
                    num2 |= 1;
                    this.WriteAMF3IntegerData(num2);
                    byte[] bytes = encoding.GetBytes(key);
                    if (bytes.Length > 0)
                    {
                        this.Write(bytes);
                    }
                }
                else
                {
                    num2 = this._objectReferences[key];
                    num2 = num2 << 1;
                    this.WriteAMF3IntegerData(num2);
                }
            }
        }

        public void WriteAMF3XElement(XElement xElement)
        {
            this.WriteByte(11);
            string key = string.Empty;
            if (xElement != null)
            {
                key = xElement.ToString();
            }
            if (key == string.Empty)
            {
                this.WriteAMF3IntegerData(1);
            }
            else
            {
                int num2;
                if (!this._objectReferences.ContainsKey(key))
                {
                    this._objectReferences.Add(key, this._objectReferences.Count);
                    UTF8Encoding encoding = new UTF8Encoding();
                    num2 = encoding.GetByteCount(key) << 1;
                    num2 |= 1;
                    this.WriteAMF3IntegerData(num2);
                    byte[] bytes = encoding.GetBytes(key);
                    if (bytes.Length > 0)
                    {
                        this.Write(bytes);
                    }
                }
                else
                {
                    num2 = this._objectReferences[key];
                    num2 = num2 << 1;
                    this.WriteAMF3IntegerData(num2);
                }
            }
        }

        public void WriteAMF3XmlDocument(XmlDocument value)
        {
            this.WriteByte(11);
            string s = string.Empty;
            if ((value.DocumentElement != null) && (value.DocumentElement.OuterXml != null))
            {
                s = value.DocumentElement.OuterXml;
            }
            if (s == string.Empty)
            {
                this.WriteAMF3IntegerData(1);
            }
            else
            {
                int num2;
                if (!this._objectReferences.ContainsKey(value))
                {
                    this._objectReferences.Add(value, this._objectReferences.Count);
                    UTF8Encoding encoding = new UTF8Encoding();
                    num2 = encoding.GetByteCount(s) << 1;
                    num2 |= 1;
                    this.WriteAMF3IntegerData(num2);
                    byte[] bytes = encoding.GetBytes(s);
                    if (bytes.Length > 0)
                    {
                        this.Write(bytes);
                    }
                }
                else
                {
                    num2 = this._objectReferences[value];
                    num2 = num2 << 1;
                    this.WriteAMF3IntegerData(num2);
                }
            }
        }

        public void WriteArray(ObjectEncoding objectEcoding, Array value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.AddReference(value);
                this.WriteByte(10);
                this.WriteInt32(value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    this.WriteData(objectEcoding, value.GetValue(i));
                }
            }
        }

        public void WriteASO(ObjectEncoding objectEncoding, ASObject asObject)
        {
            if (asObject != null)
            {
                this.AddReference(asObject);
                if (asObject.TypeName == null)
                {
                    this.BaseStream.WriteByte(3);
                }
                else
                {
                    this.BaseStream.WriteByte(0x10);
                    this.WriteUTF(asObject.TypeName);
                }
                foreach (KeyValuePair<string, object> pair in asObject)
                {
                    this.WriteUTF(pair.Key.ToString());
                    this.WriteData(objectEncoding, pair.Value);
                }
                this.WriteEndMarkup();
            }
            else
            {
                this.WriteNull();
            }
        }

        public void WriteAssociativeArray(ObjectEncoding objectEncoding, IDictionary value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.AddReference(value);
                this.WriteByte(8);
                this.WriteInt32(value.Count);
                foreach (DictionaryEntry entry in value)
                {
                    this.WriteUTF(entry.Key.ToString());
                    this.WriteData(objectEncoding, entry.Value);
                }
                this.WriteEndMarkup();
            }
        }

        private void WriteBigEndian(byte[] bytes)
        {
            if (bytes != null)
            {
                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    base.BaseStream.WriteByte(bytes[i]);
                }
            }
        }

        public void WriteBoolean(bool value)
        {
            this.BaseStream.WriteByte(value ? ((byte) 1) : ((byte) 0));
        }

        public void WriteByte(byte value)
        {
            this.BaseStream.WriteByte(value);
        }

        internal void WriteByteArray(ByteArray byteArray)
        {
            this._objectReferences.Add(byteArray, this._objectReferences.Count);
            this.WriteByte(12);
            int num = (int) (byteArray.Length << 1);
            num |= 1;
            this.WriteAMF3IntegerData(num);
            this.WriteBytes(byteArray.MemoryStream.ToArray());
        }

        public void WriteBytes(byte[] buffer)
        {
            for (int i = 0; (buffer != null) && (i < buffer.Length); i++)
            {
                this.BaseStream.WriteByte(buffer[i]);
            }
        }

        public void WriteData(ObjectEncoding objectEncoding, object data)
        {
            if (data == null)
            {
                this.WriteNull();
            }
            else
            {
                Type type = data.GetType();
                if ((FluorineConfiguration.Instance.AcceptNullValueTypes && (FluorineConfiguration.Instance.NullableValues != null)) && (FluorineConfiguration.Instance.NullableValues.ContainsKey(type) && data.Equals(FluorineConfiguration.Instance.NullableValues[type])))
                {
                    this.WriteNull();
                }
                else if (this._amf0ObjectReferences.ContainsKey(data))
                {
                    this.WriteReference(data);
                }
                else
                {
                    IAMFWriter writer = null;
                    if (AmfWriterTable[0].ContainsKey(type))
                    {
                        writer = AmfWriterTable[0][type];
                    }
                    if ((writer == null) && AmfWriterTable[0].ContainsKey(type.BaseType))
                    {
                        writer = AmfWriterTable[0][type.BaseType];
                    }
                    if (writer == null)
                    {
                        lock (AmfWriterTable)
                        {
                            if (!AmfWriterTable[0].ContainsKey(type))
                            {
                                writer = new AMF0ObjectWriter();
                                AmfWriterTable[0].Add(type, writer);
                            }
                            else
                            {
                                writer = AmfWriterTable[0][type];
                            }
                        }
                    }
                    if (writer == null)
                    {
                        throw new FluorineException(__Res.GetString("TypeSerializer_NotFound", new object[] { type.FullName }));
                    }
                    if (objectEncoding == ObjectEncoding.AMF0)
                    {
                        writer.WriteData(this, data);
                    }
                    else if (writer.IsPrimitive)
                    {
                        writer.WriteData(this, data);
                    }
                    else
                    {
                        this.WriteByte(0x11);
                        this.WriteAMF3Data(data);
                    }
                }
            }
        }

        public void WriteDateTime(DateTime value)
        {
            value = value.ToUniversalTime();
            DateTime time = new DateTime(0x7b2, 1, 1);
            long totalMilliseconds = (long) value.Subtract(time).TotalMilliseconds;
            this.WriteDouble((double) totalMilliseconds);
            TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(value);
            if (FluorineConfiguration.Instance.TimezoneCompensation == TimezoneCompensation.None)
            {
                this.WriteShort(0);
            }
            else
            {
                this.WriteShort((int) (utcOffset.TotalMilliseconds / 60000.0));
            }
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBigEndian(bytes);
        }

        internal void WriteEndMarkup()
        {
            base.BaseStream.WriteByte(0);
            base.BaseStream.WriteByte(0);
            base.BaseStream.WriteByte(9);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBigEndian(bytes);
        }

        public void WriteInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBigEndian(bytes);
        }

        public void WriteLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBigEndian(bytes);
        }

        private void WriteLongUTF(string value)
        {
            UTF8Encoding encoding = new UTF8Encoding(true, true);
            uint byteCount = (uint) encoding.GetByteCount(value);
            byte[] bytes = new byte[byteCount + 4];
            bytes[0] = (byte) ((byteCount >> 0x18) & 0xff);
            bytes[1] = (byte) ((byteCount >> 0x10) & 0xff);
            bytes[2] = (byte) ((byteCount >> 8) & 0xff);
            bytes[3] = (byte) (byteCount & 0xff);
            int num2 = encoding.GetBytes(value, 0, value.Length, bytes, 4);
            if (bytes.Length > 0)
            {
                base.BaseStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void WriteNull()
        {
            this.WriteByte(5);
        }

        public void WriteObject(ObjectEncoding objectEncoding, object obj)
        {
            if (obj == null)
            {
                this.WriteNull();
            }
            else
            {
                int num;
                FieldInfo info2;
                this.AddReference(obj);
                Type type = obj.GetType();
                this.WriteByte(0x10);
                string fullName = type.FullName;
                fullName = FluorineConfiguration.Instance.GetCustomClass(fullName);
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("TypeMapping_Write", new object[] { type.FullName, fullName }));
                }
                this.WriteUTF(fullName);
                List<PropertyInfo> list = new List<PropertyInfo>(obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance));
                for (num = list.Count - 1; num >= 0; num--)
                {
                    PropertyInfo info = list[num];
                    if (info.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                    {
                        list.RemoveAt(num);
                    }
                    if (info.GetCustomAttributes(typeof(TransientAttribute), true).Length > 0)
                    {
                        list.RemoveAt(num);
                    }
                }
                foreach (PropertyInfo info in list)
                {
                    this.WriteUTF(info.Name);
                    object data = info.GetValue(obj, null);
                    this.WriteData(objectEncoding, data);
                }
                List<FieldInfo> list2 = new List<FieldInfo>(obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance));
                for (num = list2.Count - 1; num >= 0; num--)
                {
                    info2 = list2[num];
                    if (info2.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                    {
                        list2.RemoveAt(num);
                    }
                    if (info2.GetCustomAttributes(typeof(TransientAttribute), true).Length > 0)
                    {
                        list2.RemoveAt(num);
                    }
                }
                for (num = 0; num < list2.Count; num++)
                {
                    info2 = list2[num];
                    this.WriteUTF(info2.Name);
                    this.WriteData(objectEncoding, info2.GetValue(obj));
                }
                this.WriteEndMarkup();
            }
        }

        internal void WriteReference(object value)
        {
            this.WriteByte(7);
            this.WriteShort(this._amf0ObjectReferences[value]);
        }

        public void WriteShort(int value)
        {
            byte[] bytes = BitConverter.GetBytes((ushort) value);
            this.WriteBigEndian(bytes);
        }

        public void WriteString(string value)
        {
            UTF8Encoding encoding = new UTF8Encoding(true, true);
            if (encoding.GetByteCount(value) < 0x10000)
            {
                this.WriteByte(2);
                this.WriteUTF(value);
            }
            else
            {
                this.WriteByte(12);
                this.WriteLongUTF(value);
            }
        }

        public void WriteUInt24(int value)
        {
            byte[] buffer = new byte[] { (byte) (0xff & (value >> 0x10)), (byte) (0xff & (value >> 8)), (byte) (0xff & value) };
            this.BaseStream.Write(buffer, 0, buffer.Length);
        }

        public void WriteUTF(string value)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            int byteCount = encoding.GetByteCount(value);
            byte[] bytes = encoding.GetBytes(value);
            this.WriteShort(byteCount);
            if (bytes.Length > 0)
            {
                base.Write(bytes);
            }
        }

        public void WriteUTFBytes(string value)
        {
            byte[] bytes = new UTF8Encoding().GetBytes(value);
            if (bytes.Length > 0)
            {
                base.Write(bytes);
            }
        }

        public void WriteXDocument(XDocument xDocument)
        {
            if (xDocument != null)
            {
                this.AddReference(xDocument);
                this.BaseStream.WriteByte(15);
                string str = xDocument.ToString();
                this.WriteLongUTF(str);
            }
            else
            {
                this.WriteNull();
            }
        }

        public void WriteXElement(XElement xElement)
        {
            if (xElement != null)
            {
                this.AddReference(xElement);
                this.BaseStream.WriteByte(15);
                string str = xElement.ToString();
                this.WriteLongUTF(str);
            }
            else
            {
                this.WriteNull();
            }
        }

        public void WriteXmlDocument(XmlDocument value)
        {
            if (value != null)
            {
                this.AddReference(value);
                this.BaseStream.WriteByte(15);
                string outerXml = value.DocumentElement.OuterXml;
                this.WriteLongUTF(outerXml);
            }
            else
            {
                this.WriteNull();
            }
        }

        public bool UseLegacyCollection
        {
            get
            {
                return this._useLegacyCollection;
            }
            set
            {
                this._useLegacyCollection = value;
            }
        }
    }
}

