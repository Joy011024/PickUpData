namespace FluorineFx.IO
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.Configuration;
    using FluorineFx.Exceptions;
    using FluorineFx.IO.Readers;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml;

    public class AMFReader : BinaryReader
    {
        private List<object> _amf0ObjectReferences;
        private List<ClassDefinition> _classDefinitions;
        private bool _faultTolerancy;
        private Exception _lastError;
        private List<object> _objectReferences;
        private List<object> _stringReferences;
        private bool _useLegacyCollection;
        private static IAMFReader[][] AmfTypeTable;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMFReader));

        static AMFReader()
        {
            IAMFReader[] readerArray3 = new IAMFReader[] { 
                new AMF0NumberReader(), new AMF0BooleanReader(), new AMF0StringReader(), new AMF0ASObjectReader(), new AMFUnknownTagReader(), new AMF0NullReader(), new AMF0NullReader(), new AMF0ReferenceReader(), new AMF0AssociativeArrayReader(), new AMFUnknownTagReader(), new AMF0ArrayReader(), new AMF0DateTimeReader(), new AMF0LongStringReader(), new AMFUnknownTagReader(), new AMFUnknownTagReader(), new AMF0XmlReader(), 
                ((FluorineConfiguration.Instance.OptimizerSettings != null) && FluorineConfiguration.Instance.FullTrust) ? ((IAMFReader) new AMF0OptimizedObjectReader()) : ((IAMFReader) new AMF0ObjectReader()), new AMF0AMF3TagReader()
             };
            IAMFReader[] readerArray = readerArray3;
            readerArray3 = new IAMFReader[] { 
                new AMF3NullReader(), new AMF3NullReader(), new AMF3BooleanFalseReader(), new AMF3BooleanTrueReader(), new AMF3IntegerReader(), new AMF3NumberReader(), new AMF3StringReader(), new AMF3XmlReader(), new AMF3DateTimeReader(), new AMF3ArrayReader(), ((FluorineConfiguration.Instance.OptimizerSettings != null) && FluorineConfiguration.Instance.FullTrust) ? ((IAMFReader) new AMF3OptimizedObjectReader()) : ((IAMFReader) new AMF3ObjectReader()), new AMF3XmlReader(), new AMF3ByteArrayReader(), new AMFUnknownTagReader(), new AMFUnknownTagReader(), new AMFUnknownTagReader(), 
                new AMFUnknownTagReader(), new AMFUnknownTagReader()
             };
            IAMFReader[] readerArray2 = readerArray3;
            IAMFReader[][] readerArray4 = new IAMFReader[4][];
            readerArray4[0] = readerArray;
            readerArray4[3] = readerArray2;
            AmfTypeTable = readerArray4;
        }

        public AMFReader(Stream stream) : base(stream)
        {
            this._useLegacyCollection = true;
            this._faultTolerancy = false;
            this.Reset();
        }

        public void AddAMF3ObjectReference(object instance)
        {
            this._objectReferences.Add(instance);
        }

        internal void AddClassReference(ClassDefinition classDefinition)
        {
            this._classDefinitions.Add(classDefinition);
        }

        public void AddReference(object instance)
        {
            this._amf0ObjectReferences.Add(instance);
        }

        internal void AddStringReference(string str)
        {
            this._stringReferences.Add(str);
        }

        public object ReadAMF3Array()
        {
            int index = this.ReadAMF3IntegerData();
            bool flag = (index & 1) != 0;
            index = index >> 1;
            if (flag)
            {
                object obj2;
                int num2;
                Dictionary<string, object> instance = null;
                for (string str = this.ReadAMF3String(); (str != null) && (str != string.Empty); str = this.ReadAMF3String())
                {
                    if (instance == null)
                    {
                        instance = new Dictionary<string, object>();
                        this.AddAMF3ObjectReference(instance);
                    }
                    obj2 = this.ReadAMF3Data();
                    instance.Add(str, obj2);
                }
                if (instance == null)
                {
                    object[] objArray = new object[index];
                    this.AddAMF3ObjectReference(objArray);
                    for (num2 = 0; num2 < index; num2++)
                    {
                        byte typeMarker = this.ReadByte();
                        obj2 = this.ReadAMF3Data(typeMarker);
                        objArray[num2] = obj2;
                    }
                    return objArray;
                }
                for (num2 = 0; num2 < index; num2++)
                {
                    obj2 = this.ReadAMF3Data();
                    instance.Add(num2.ToString(), obj2);
                }
                return instance;
            }
            return this.ReadAMF3ObjectReference(index);
        }

        [CLSCompliant(false)]
        public ByteArray ReadAMF3ByteArray()
        {
            int index = this.ReadAMF3IntegerData();
            bool flag = (index & 1) != 0;
            index = index >> 1;
            if (flag)
            {
                int count = index;
                ByteArray instance = new ByteArray(this.ReadBytes(count));
                this.AddAMF3ObjectReference(instance);
                return instance;
            }
            return (this.ReadAMF3ObjectReference(index) as ByteArray);
        }

        public object ReadAMF3Data()
        {
            byte typeMarker = this.ReadByte();
            return this.ReadAMF3Data(typeMarker);
        }

        public object ReadAMF3Data(byte typeMarker)
        {
            return AmfTypeTable[3][typeMarker].ReadData(this);
        }

        public DateTime ReadAMF3Date()
        {
            int index = this.ReadAMF3IntegerData();
            bool flag = (index & 1) != 0;
            index = index >> 1;
            if (flag)
            {
                double num2 = this.ReadDouble();
                DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                DateTime instance = DateTime.SpecifyKind(time.AddMilliseconds(num2), DateTimeKind.Utc);
                this.AddAMF3ObjectReference(instance);
                return instance;
            }
            return (DateTime) this.ReadAMF3ObjectReference(index);
        }

        public int ReadAMF3Int()
        {
            return this.ReadAMF3IntegerData();
        }

        public int ReadAMF3IntegerData()
        {
            int num = this.ReadByte();
            if (num < 0x80)
            {
                return num;
            }
            num = (num & 0x7f) << 7;
            int num2 = this.ReadByte();
            if (num2 < 0x80)
            {
                num |= num2;
            }
            else
            {
                num = (num | (num2 & 0x7f)) << 7;
                num2 = this.ReadByte();
                if (num2 < 0x80)
                {
                    num |= num2;
                }
                else
                {
                    num = (num | (num2 & 0x7f)) << 8;
                    num2 = this.ReadByte();
                    num |= num2;
                }
            }
            int num3 = 0x10000000;
            return (-(num & num3) | num);
        }

        public object ReadAMF3Object()
        {
            int index = this.ReadAMF3IntegerData();
            bool flag = (index & 1) != 0;
            index = index >> 1;
            if (!flag)
            {
                return this.ReadAMF3ObjectReference(index);
            }
            ClassDefinition classDefinition = this.ReadClassDefinition(index);
            return this.ReadAMF3Object(classDefinition);
        }

        internal object ReadAMF3Object(ClassDefinition classDefinition)
        {
            object instance = null;
            string name;
            object obj3;
            if ((classDefinition.ClassName != null) && (classDefinition.ClassName != string.Empty))
            {
                instance = ObjectFactory.CreateInstance(classDefinition.ClassName);
            }
            else
            {
                instance = new ASObject();
            }
            if (instance == null)
            {
                if (log.get_IsWarnEnabled())
                {
                    log.Warn(__Res.GetString("TypeLoad_ASO", new object[] { classDefinition.ClassName }));
                }
                instance = new ASObject(classDefinition.ClassName);
            }
            this.AddAMF3ObjectReference(instance);
            if (classDefinition.IsExternalizable)
            {
                if (!(instance is IExternalizable))
                {
                    throw new FluorineException(__Res.GetString("Externalizable_CastFail", new object[] { instance.GetType().FullName }));
                }
                IExternalizable externalizable = instance as IExternalizable;
                DataInput input = new DataInput(this);
                externalizable.ReadExternal(input);
                return instance;
            }
            for (int i = 0; i < classDefinition.MemberCount; i++)
            {
                name = classDefinition.Members[i].Name;
                obj3 = this.ReadAMF3Data();
                this.SetMember(instance, name, obj3);
            }
            if (classDefinition.IsDynamic)
            {
                for (name = this.ReadAMF3String(); (name != null) && (name != string.Empty); name = this.ReadAMF3String())
                {
                    obj3 = this.ReadAMF3Data();
                    this.SetMember(instance, name, obj3);
                }
            }
            return instance;
        }

        public object ReadAMF3ObjectReference(int index)
        {
            return this._objectReferences[index];
        }

        public string ReadAMF3String()
        {
            int index = this.ReadAMF3IntegerData();
            bool flag = (index & 1) != 0;
            index = index >> 1;
            if (flag)
            {
                int length = index;
                if (length == 0)
                {
                    return string.Empty;
                }
                string str = this.ReadUTF(length);
                this.AddStringReference(str);
                return str;
            }
            return this.ReadStringReference(index);
        }

        public XmlDocument ReadAMF3XmlDocument()
        {
            int length = this.ReadAMF3IntegerData();
            bool flag = (length & 1) != 0;
            length = length >> 1;
            string instance = string.Empty;
            if (flag)
            {
                if (length > 0)
                {
                    instance = this.ReadUTF(length);
                }
                this.AddAMF3ObjectReference(instance);
            }
            else
            {
                instance = this.ReadAMF3ObjectReference(length) as string;
            }
            XmlDocument document = new XmlDocument();
            if ((instance != null) && (instance != string.Empty))
            {
                document.LoadXml(instance);
            }
            return document;
        }

        internal IList ReadArray()
        {
            int num = this.ReadInt32();
            object[] instance = new object[num];
            this.AddReference(instance);
            for (int i = 0; i < num; i++)
            {
                instance[i] = this.ReadData();
            }
            return instance;
        }

        public ASObject ReadASObject()
        {
            ASObject instance = new ASObject();
            this.AddReference(instance);
            string str = this.ReadString();
            for (byte i = this.ReadByte(); i != 9; i = this.ReadByte())
            {
                instance[str] = this.ReadData(i);
                str = this.ReadString();
            }
            return instance;
        }

        internal Dictionary<string, object> ReadAssociativeArray()
        {
            Dictionary<string, object> instance = new Dictionary<string, object>(this.ReadInt32());
            this.AddReference(instance);
            string key = this.ReadString();
            for (byte i = this.ReadByte(); i != 9; i = this.ReadByte())
            {
                object obj2 = this.ReadData(i);
                instance.Add(key, obj2);
                key = this.ReadString();
            }
            return instance;
        }

        public override bool ReadBoolean()
        {
            return base.ReadBoolean();
        }

        internal ClassDefinition ReadClassDefinition(int handle)
        {
            ClassDefinition classDefinition = null;
            bool flag = (handle & 1) != 0;
            handle = handle >> 1;
            if (flag)
            {
                string className = this.ReadAMF3String();
                bool externalizable = (handle & 1) != 0;
                handle = handle >> 1;
                bool dynamic = (handle & 1) != 0;
                handle = handle >> 1;
                ClassMember[] members = new ClassMember[handle];
                for (int i = 0; i < handle; i++)
                {
                    members[i] = new ClassMember(this.ReadAMF3String(), BindingFlags.Default, MemberTypes.Custom);
                }
                classDefinition = new ClassDefinition(className, members, externalizable, dynamic);
                this.AddClassReference(classDefinition);
            }
            else
            {
                classDefinition = this.ReadClassReference(handle);
            }
            if (log.get_IsDebugEnabled())
            {
                if (classDefinition.IsTypedObject)
                {
                    log.Debug(__Res.GetString("ClassDefinition_Loaded", new object[] { classDefinition.ClassName }));
                    return classDefinition;
                }
                log.Debug(__Res.GetString("ClassDefinition_LoadedUntyped"));
            }
            return classDefinition;
        }

        internal ClassDefinition ReadClassReference(int index)
        {
            return this._classDefinitions[index];
        }

        public object ReadData()
        {
            byte typeMarker = this.ReadByte();
            return this.ReadData(typeMarker);
        }

        public object ReadData(byte typeMarker)
        {
            return AmfTypeTable[0][typeMarker].ReadData(this);
        }

        public DateTime ReadDateTime()
        {
            double num = this.ReadDouble();
            DateTime time = new DateTime(0x7b2, 1, 1);
            DateTime time2 = DateTime.SpecifyKind(time.AddMilliseconds(num), DateTimeKind.Utc);
            int num2 = this.ReadUInt16();
            if (num2 > 720)
            {
                num2 = 0x10000 - num2;
            }
            int num3 = num2 / 60;
            switch (FluorineConfiguration.Instance.TimezoneCompensation)
            {
                case TimezoneCompensation.None:
                    return time2;

                case TimezoneCompensation.Auto:
                    return DateTime.SpecifyKind(time2.AddHours((double) num3), DateTimeKind.Unspecified);
            }
            return time2;
        }

        public override double ReadDouble()
        {
            byte[] buffer = this.ReadBytes(8);
            byte[] buffer2 = new byte[8];
            int index = 7;
            for (int i = 0; index >= 0; i++)
            {
                buffer2[i] = buffer[index];
                index--;
            }
            return BitConverter.ToDouble(buffer2, 0);
        }

        public float ReadFloat()
        {
            byte[] buffer = this.ReadBytes(4);
            byte[] buffer2 = new byte[4];
            int index = 3;
            for (int i = 0; index >= 0; i++)
            {
                buffer2[i] = buffer[index];
                index--;
            }
            return BitConverter.ToSingle(buffer2, 0);
        }

        public override short ReadInt16()
        {
            byte[] buffer = this.ReadBytes(2);
            return (short) ((buffer[0] << 8) | buffer[1]);
        }

        public override int ReadInt32()
        {
            byte[] buffer = this.ReadBytes(4);
            return ((((buffer[0] << 0x18) | (buffer[1] << 0x10)) | (buffer[2] << 8)) | buffer[3]);
        }

        public string ReadLongString()
        {
            int length = this.ReadInt32();
            return this.ReadUTF(length);
        }

        public object ReadObject()
        {
            string typeName = this.ReadString();
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("TypeIdentifier_Loaded", new object[] { typeName }));
            }
            Type type = ObjectFactory.Locate(typeName);
            if (type != null)
            {
                object instance = ObjectFactory.CreateInstance(type);
                this.AddReference(instance);
                string memberName = this.ReadString();
                for (byte i = this.ReadByte(); i != 9; i = this.ReadByte())
                {
                    object obj3 = this.ReadData(i);
                    this.SetMember(instance, memberName, obj3);
                    memberName = this.ReadString();
                }
                return instance;
            }
            if (log.get_IsWarnEnabled())
            {
                log.Warn(__Res.GetString("TypeLoad_ASO", new object[] { typeName }));
            }
            ASObject obj4 = this.ReadASObject();
            obj4.TypeName = typeName;
            return obj4;
        }

        public object ReadReference()
        {
            int num = this.ReadUInt16();
            return this._amf0ObjectReferences[num];
        }

        public override string ReadString()
        {
            int length = this.ReadUInt16();
            return this.ReadUTF(length);
        }

        internal string ReadStringReference(int index)
        {
            return (this._stringReferences[index] as string);
        }

        [CLSCompliant(false)]
        public override ushort ReadUInt16()
        {
            byte[] buffer = this.ReadBytes(2);
            return (ushort) (((buffer[0] & 0xff) << 8) | (buffer[1] & 0xff));
        }

        public int ReadUInt24()
        {
            byte[] buffer = this.ReadBytes(3);
            return (((buffer[0] << 0x10) | (buffer[1] << 8)) | buffer[2]);
        }

        public string ReadUTF(int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }
            UTF8Encoding encoding = new UTF8Encoding(false, true);
            byte[] bytes = this.ReadBytes(length);
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public XmlDocument ReadXmlDocument()
        {
            string xml = this.ReadLongString();
            XmlDocument document = new XmlDocument();
            if ((xml != null) && (xml != string.Empty))
            {
                document.LoadXml(xml);
            }
            return document;
        }

        public void Reset()
        {
            this._amf0ObjectReferences = new List<object>(5);
            this._objectReferences = new List<object>(15);
            this._stringReferences = new List<object>(15);
            this._classDefinitions = new List<ClassDefinition>(2);
            this._lastError = null;
        }

        internal void SetMember(object instance, string memberName, object value)
        {
            if (instance is ASObject)
            {
                ((ASObject) instance)[memberName] = value;
            }
            else
            {
                string str;
                Exception exception;
                Type type = instance.GetType();
                PropertyInfo property = null;
                try
                {
                    property = type.GetProperty(memberName);
                }
                catch (AmbiguousMatchException)
                {
                    property = type.GetProperty(memberName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                }
                if (property != null)
                {
                    try
                    {
                        value = TypeHelper.ChangeType(value, property.PropertyType);
                        if (property.CanWrite)
                        {
                            if ((property.GetIndexParameters() == null) || (property.GetIndexParameters().Length == 0))
                            {
                                property.SetValue(instance, value, null);
                            }
                            else
                            {
                                str = __Res.GetString("Reflection_PropertyIndexFail", new object[] { string.Format("{0}.{1}", type.FullName, memberName) });
                                if (log.get_IsErrorEnabled())
                                {
                                    log.Error(str);
                                }
                                if (!this._faultTolerancy)
                                {
                                    throw new FluorineException(str);
                                }
                                this._lastError = new FluorineException(str);
                            }
                        }
                        else
                        {
                            str = __Res.GetString("Reflection_PropertyReadOnly", new object[] { string.Format("{0}.{1}", type.FullName, memberName) });
                            if (log.get_IsWarnEnabled())
                            {
                                log.Warn(str);
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        str = __Res.GetString("Reflection_PropertySetFail", new object[] { string.Format("{0}.{1}", type.FullName, memberName), exception.Message });
                        if (log.get_IsErrorEnabled())
                        {
                            log.Error(str, exception);
                        }
                        if (!this._faultTolerancy)
                        {
                            throw new FluorineException(str);
                        }
                        this._lastError = new FluorineException(str);
                    }
                }
                else
                {
                    FieldInfo field = type.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);
                    try
                    {
                        if (field != null)
                        {
                            value = TypeHelper.ChangeType(value, field.FieldType);
                            field.SetValue(instance, value);
                        }
                        else
                        {
                            str = __Res.GetString("Reflection_MemberNotFound", new object[] { string.Format("{0}.{1}", type.FullName, memberName) });
                            if (log.get_IsWarnEnabled())
                            {
                                log.Warn(str);
                            }
                        }
                    }
                    catch (Exception exception3)
                    {
                        exception = exception3;
                        str = __Res.GetString("Reflection_FieldSetFail", new object[] { string.Format("{0}.{1}", type.FullName, memberName), exception.Message });
                        if (log.get_IsErrorEnabled())
                        {
                            log.Error(str, exception);
                        }
                        if (!this._faultTolerancy)
                        {
                            throw new FluorineException(str);
                        }
                        this._lastError = new FluorineException(str);
                    }
                }
            }
        }

        public bool FaultTolerancy
        {
            get
            {
                return this._faultTolerancy;
            }
            set
            {
                this._faultTolerancy = value;
            }
        }

        public Exception LastError
        {
            get
            {
                return this._lastError;
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

