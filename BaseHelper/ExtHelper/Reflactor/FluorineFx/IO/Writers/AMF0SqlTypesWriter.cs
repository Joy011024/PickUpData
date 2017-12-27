namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using log4net;
    using System;
    using System.Data.SqlTypes;

    internal class AMF0SqlTypesWriter : IAMFWriter
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AMF0SqlTypesWriter));

        public void WriteData(AMFWriter writer, object data)
        {
            if ((data is INullable) && (data as INullable).IsNull)
            {
                writer.WriteNull();
            }
            else if (data is SqlByte)
            {
                SqlByte num = (SqlByte) data;
                writer.WriteData(ObjectEncoding.AMF0, num.Value);
            }
            else if (data is SqlInt16)
            {
                SqlInt16 num2 = (SqlInt16) data;
                writer.WriteData(ObjectEncoding.AMF0, num2.Value);
            }
            else if (data is SqlInt32)
            {
                SqlInt32 num3 = (SqlInt32) data;
                writer.WriteData(ObjectEncoding.AMF0, num3.Value);
            }
            else if (data is SqlInt64)
            {
                SqlInt64 num4 = (SqlInt64) data;
                writer.WriteData(ObjectEncoding.AMF0, num4.Value);
            }
            else if (data is SqlSingle)
            {
                SqlSingle num5 = (SqlSingle) data;
                writer.WriteData(ObjectEncoding.AMF0, num5.Value);
            }
            else if (data is SqlDouble)
            {
                SqlDouble num6 = (SqlDouble) data;
                writer.WriteData(ObjectEncoding.AMF0, num6.Value);
            }
            else if (data is SqlDecimal)
            {
                SqlDecimal num7 = (SqlDecimal) data;
                writer.WriteData(ObjectEncoding.AMF0, num7.Value);
            }
            else if (data is SqlMoney)
            {
                SqlMoney money = (SqlMoney) data;
                writer.WriteData(ObjectEncoding.AMF0, money.Value);
            }
            else if (data is SqlDateTime)
            {
                SqlDateTime time = (SqlDateTime) data;
                writer.WriteData(ObjectEncoding.AMF0, time.Value);
            }
            else if (data is SqlString)
            {
                SqlString str2 = (SqlString) data;
                writer.WriteString(str2.Value);
            }
            else if (data is SqlGuid)
            {
                SqlGuid guid = (SqlGuid) data;
                writer.WriteData(ObjectEncoding.AMF0, guid.Value.ToString("N"));
            }
            else if (data is SqlBoolean)
            {
                SqlBoolean flag2 = (SqlBoolean) data;
                writer.WriteBoolean(flag2.Value);
            }
            else
            {
                string message = string.Format("Could not find serializer for type {0}", data.GetType().FullName);
                if (_log.get_IsErrorEnabled())
                {
                    _log.Error(message);
                }
                throw new FluorineException(message);
            }
        }

        public bool IsPrimitive
        {
            get
            {
                return true;
            }
        }
    }
}

