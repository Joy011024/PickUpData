namespace FluorineFx.SWX
{
    using FluorineFx;
    using FluorineFx.HttpCompress;
    using FluorineFx.SWX.Writers;
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Text;

    internal class SwxAssembler
    {
        private ByteBuffer _swf = ByteBuffer.Allocate(50);
        private static byte[] ActionDoAction;
        private static byte[] ActionEndSwf;
        private static byte ActionInitArray;
        private static byte ActionInitObject;
        private static byte[] ActionPush;
        public static byte ActionPushData;
        private static byte ActionSetVariable;
        private static byte[] ActionShowFrame;
        private static byte[] AllowDomain;
        private static byte CompressedSwf = 0x43;
        public static byte DataTypeBoolean;
        public static byte DataTypeConstantPool1;
        public static byte DataTypeDouble;
        public static byte DataTypeFloat;
        public static byte DataTypeInteger;
        public static byte DataTypeNull;
        public static byte DataTypeString;
        private static byte[] DebugEnd;
        private static byte[] DebugStart;
        public static byte NullTerminator;
        private static byte[] SwfHeader = new byte[] { 
            0x57, 0x53, 6, 0, 0, 0, 0, 0x30, 10, 0, 160, 0, 1, 1, 0, 0x43, 
            2, 0xff, 0xff, 0xff
         };
        private static Hashtable SWXWriterTable;
        private static byte[] SystemAllowDomain;
        private static byte UncompressedSwf = 70;

        static SwxAssembler()
        {
            byte[] buffer = new byte[3];
            buffer[0] = 150;
            ActionPush = buffer;
            buffer = new byte[2];
            buffer[0] = 0x40;
            ActionShowFrame = buffer;
            buffer = new byte[2];
            ActionEndSwf = buffer;
            ActionSetVariable = 0x1d;
            ActionDoAction = new byte[] { 0x3f, 3 };
            ActionInitArray = 0x42;
            ActionInitObject = 0x43;
            ActionPushData = 150;
            DataTypeString = 0;
            DataTypeFloat = 1;
            DataTypeNull = 2;
            DataTypeBoolean = 5;
            DataTypeDouble = 6;
            DataTypeInteger = 7;
            DataTypeConstantPool1 = 8;
            NullTerminator = 0;
            AllowDomain = new byte[] { 
                150, 9, 0, 0, 0x5f, 0x70, 0x61, 0x72, 0x65, 110, 0x74, 0, 0x1c, 150, 6, 0, 
                0, 0x5f, 0x75, 0x72, 0x6c, 0, 0x4e, 150, 13, 0, 7, 1, 0, 0, 0, 0, 
                0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0, 0x1c, 150, 10, 0, 0, 0x73, 0x65, 0x63, 0x75, 
                0x72, 0x69, 0x74, 0x79, 0, 0x4e, 150, 13, 0, 0, 0x61, 0x6c, 0x6c, 0x6f, 0x77, 0x44, 
                0x6f, 0x6d, 0x61, 0x69, 110, 0, 0x52, 0x17
             };
            SystemAllowDomain = new byte[] { 
                7, 1, 0, 0, 0, 0, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0, 0x1c, 150, 10, 
                0, 0, 0x73, 0x65, 0x63, 0x75, 0x72, 0x69, 0x74, 0x79, 0, 0x4e, 150, 13, 0, 0, 
                0x61, 0x6c, 0x6c, 0x6f, 0x77, 0x44, 0x6f, 0x6d, 0x61, 0x69, 110, 0, 0x52, 0x17
             };
            DebugStart = new byte[] { 
                0x88, 60, 0, 7, 0, 0x72, 0x65, 0x73, 0x75, 0x6c, 0x74, 0, 0x6c, 0x63, 0, 0x4c, 
                0x6f, 0x63, 0x61, 0x6c, 0x43, 0x6f, 110, 110, 0x65, 0x63, 0x74, 0x69, 0x6f, 110, 0, 0x5f, 
                0x73, 0x77, 120, 0x44, 0x65, 0x62, 0x75, 0x67, 0x67, 0x65, 0x72, 0, 0x63, 0x6f, 110, 110, 
                0x65, 0x63, 0x74, 0, 100, 0x65, 0x62, 0x75, 0x67, 0, 0x73, 0x65, 110, 100, 0
             };
            DebugEnd = new byte[] { 
                150, 13, 0, 8, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 8, 2, 
                0x40, 60, 150, 9, 0, 8, 3, 7, 1, 0, 0, 0, 8, 1, 0x1c, 150, 
                2, 0, 8, 4, 0x52, 0x17, 150, 2, 0, 8, 0, 0x1c, 150, 5, 0, 7, 
                1, 0, 0, 0, 0x42, 150, 11, 0, 8, 5, 8, 3, 7, 3, 0, 0, 
                0, 8, 1, 0x1c, 150, 2, 0, 8, 6, 0x52, 0x17
             };
            SWXWriterTable = new Hashtable();
            SWXDoubleWriter writer = new SWXDoubleWriter();
            SWXWriterTable.Add(typeof(sbyte), writer);
            SWXWriterTable.Add(typeof(byte), writer);
            SWXWriterTable.Add(typeof(short), writer);
            SWXWriterTable.Add(typeof(ushort), writer);
            SWXWriterTable.Add(typeof(int), writer);
            SWXWriterTable.Add(typeof(uint), writer);
            SWXWriterTable.Add(typeof(long), writer);
            SWXWriterTable.Add(typeof(ulong), writer);
            SWXWriterTable.Add(typeof(float), writer);
            SWXWriterTable.Add(typeof(double), writer);
            SWXWriterTable.Add(typeof(decimal), writer);
            SWXWriterTable.Add(typeof(bool), new SWXBooleanWriter());
            SWXWriterTable.Add(typeof(string), new SWXStringWriter());
            SWXWriterTable.Add(typeof(Array), new SWXArrayWriter());
        }

        internal void DataToBytecode(object data)
        {
            if (this._swf.Bookmark != -1L)
            {
                int num = (int) (this._swf.Position - this._swf.Bookmark);
                if (num >= 0xfff0)
                {
                    this.EndPush();
                    this._swf.Put(ActionPushData);
                    this._swf.SetBookmark();
                    this._swf.Skip(2);
                }
            }
            else
            {
                this._swf.Put(ActionPushData);
                this._swf.SetBookmark();
                this._swf.Skip(2);
            }
            if (data == null)
            {
                this.PushNull();
            }
            else
            {
                Type key = data.GetType();
                ISWXWriter writer = SWXWriterTable[key] as ISWXWriter;
                if (writer == null)
                {
                    writer = SWXWriterTable[key.BaseType] as ISWXWriter;
                }
                if (writer == null)
                {
                    lock (SWXWriterTable)
                    {
                        if (!SWXWriterTable.Contains(key))
                        {
                            writer = new SWXObjectWriter();
                            SWXWriterTable.Add(key, writer);
                        }
                        else
                        {
                            writer = SWXWriterTable[key] as ISWXWriter;
                        }
                    }
                }
                writer.WriteData(this, data);
            }
        }

        private void EndPush()
        {
            if (this._swf.Bookmark != -1L)
            {
                this._swf.Put((int) this._swf.Bookmark, (ushort) ((this._swf.Position - this._swf.Bookmark) - 2L));
                this._swf.ClearBookmark();
            }
        }

        private void GenerateAllowDomainBytecode(string url)
        {
            if (url != null)
            {
                this._swf.Put(ActionPushData);
                int position = (int) this._swf.Position;
                this._swf.Skip(2);
                this.PushString(url);
                this._swf.Put(position, (ushort) (((this._swf.Position - position) - 2L) + 13L));
                this._swf.Put(SystemAllowDomain);
            }
        }

        internal void PushArray(Array value)
        {
            for (int i = value.Length - 1; i >= 0; i--)
            {
                object data = value.GetValue(i);
                this.DataToBytecode(data);
            }
            this.StartPush();
            this.PushInteger(value.Length);
            this.EndPush();
            this._swf.Put(ActionInitArray);
        }

        internal void PushAssociativeArray(IDictionary dictionary)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                this.DataToBytecode(entry.Key);
                this.DataToBytecode(entry.Value);
            }
            this.StartPush();
            this.PushInteger(dictionary.Count);
            this.EndPush();
            this._swf.Put(ActionInitObject);
        }

        internal void PushBoolean(bool value)
        {
            this._swf.Put(DataTypeBoolean);
            this._swf.Put(value ? ((byte) 1) : ((byte) 0));
        }

        internal void PushDouble(double value)
        {
            this._swf.Put(DataTypeDouble);
            byte[] bytes = BitConverter.GetBytes(value);
            this._swf.Put(bytes, 4, 4);
            this._swf.Put(bytes, 0, 4);
        }

        internal void PushInteger(int value)
        {
            this._swf.Put(DataTypeInteger);
            byte[] bytes = BitConverter.GetBytes(value);
            this._swf.Put(bytes);
        }

        internal void PushNull()
        {
            this._swf.Put(DataTypeNull);
        }

        internal void PushObject(object obj)
        {
            int num;
            object obj2;
            FieldInfo info2;
            Type type = obj.GetType();
            string fullName = type.FullName;
            ArrayList list = new ArrayList(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            for (num = list.Count - 1; num >= 0; num--)
            {
                PropertyInfo info = list[num] as PropertyInfo;
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
                this.DataToBytecode(info.Name);
                obj2 = info.GetValue(obj, null);
                this.DataToBytecode(obj2);
            }
            ArrayList list2 = new ArrayList(type.GetFields(BindingFlags.Public | BindingFlags.Instance));
            for (num = list2.Count - 1; num >= 0; num--)
            {
                info2 = list2[num] as FieldInfo;
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
                info2 = list2[num] as FieldInfo;
                this.DataToBytecode(info2.Name);
                obj2 = info2.GetValue(obj);
                this.DataToBytecode(obj2);
            }
            this.StartPush();
            this.PushInteger(list.Count + list2.Count);
            this.EndPush();
            this._swf.Put(ActionInitObject);
        }

        internal void PushString(string value)
        {
            this._swf.Put(DataTypeString);
            byte[] bytes = new UTF8Encoding().GetBytes(value);
            this._swf.Put(bytes);
            this._swf.Put(NullTerminator);
        }

        private void StartPush()
        {
            if (this._swf.Bookmark == -1L)
            {
                this._swf.Put(ActionPushData);
                this._swf.SetBookmark();
                this._swf.Skip(2);
            }
        }

        internal byte[] WriteSwf(object data, bool debug, CompressionLevels compressionLevel, string url, bool allowDomain)
        {
            byte num = (compressionLevel != CompressionLevels.None) ? CompressedSwf : UncompressedSwf;
            this._swf.Put(num);
            this._swf.Put(SwfHeader);
            this._swf.Put(ActionDoAction);
            int position = (int) this._swf.Position;
            this._swf.Skip(4);
            int num3 = (int) this._swf.Position;
            if (debug)
            {
                this._swf.Put(DebugStart);
            }
            this._swf.Put(ActionPushData);
            this._swf.SetBookmark();
            this._swf.Skip(2);
            if (debug)
            {
                this._swf.Put(DataTypeConstantPool1);
                this._swf.Put((byte) 0);
            }
            else
            {
                this.PushString("result");
            }
            this.DataToBytecode(data);
            this.EndPush();
            this._swf.Put(ActionSetVariable);
            if (allowDomain)
            {
                this.GenerateAllowDomainBytecode(url);
            }
            if (debug)
            {
                this._swf.Put(DebugEnd);
            }
            uint num5 = (uint) (this._swf.Position - num3);
            this._swf.Put(position, num5);
            this._swf.Put(ActionShowFrame);
            this._swf.Put(ActionEndSwf);
            uint length = (uint) this._swf.Length;
            this._swf.Put(4, length);
            this._swf.Flip();
            byte[] buffer = this._swf.ToArray();
            if (compressionLevel != CompressionLevels.None)
            {
                MemoryStream stream = new MemoryStream();
                DeflateStream stream2 = new DeflateStream(stream, CompressionMode.Compress, false);
                stream2.Write(buffer, 8, buffer.Length - 8);
                stream2.Close();
                byte[] src = stream.ToArray();
                byte[] dst = new byte[src.Length + 8];
                Buffer.BlockCopy(buffer, 0, dst, 0, 8);
                Buffer.BlockCopy(src, 0, dst, 8, src.Length);
                buffer = dst;
            }
            return buffer;
        }
    }
}

