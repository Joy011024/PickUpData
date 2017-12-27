namespace FluorineFx
{
    using FluorineFx.Configuration;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    public sealed class TypeHelper
    {
        private static bool _defaultBooleanNullValue = ((bool) GetNullValue(typeof(bool)));
        private static byte _defaultByteNullValue = ((byte) GetNullValue(typeof(byte)));
        private static char _defaultCharNullValue = ((char) GetNullValue(typeof(char)));
        private static DateTime _defaultDateTimeNullValue = ((DateTime) GetNullValue(typeof(DateTime)));
        private static decimal _defaultDecimalNullValue = ((decimal) GetNullValue(typeof(decimal)));
        private static double _defaultDoubleNullValue = ((double) GetNullValue(typeof(double)));
        private static Guid _defaultGuidNullValue = ((Guid) GetNullValue(typeof(Guid)));
        private static short _defaultInt16NullValue = ((short) GetNullValue(typeof(short)));
        private static int _defaultInt32NullValue = ((int) GetNullValue(typeof(int)));
        private static long _defaultInt64NullValue = ((long) GetNullValue(typeof(long)));
        private static sbyte _defaultSByteNullValue = ((sbyte) GetNullValue(typeof(sbyte)));
        private static float _defaultSingleNullValue = ((float) GetNullValue(typeof(float)));
        private static string _defaultStringNullValue = ((string) GetNullValue(typeof(string)));
        private static ushort _defaultUInt16NullValue = ((ushort) GetNullValue(typeof(ushort)));
        private static uint _defaultUInt32NullValue = ((uint) GetNullValue(typeof(uint)));
        private static ulong _defaultUInt64NullValue = ((ulong) GetNullValue(typeof(ulong)));
        private static XDocument _defaultXDocumentNullValue = ((XDocument) GetNullValue(typeof(XDocument)));
        private static XElement _defaultXElementNullValue = ((XElement) GetNullValue(typeof(XElement)));
        private static XmlDocument _defaultXmlDocumentNullValue = ((XmlDocument) GetNullValue(typeof(XmlDocument)));
        private static XmlReader _defaultXmlReaderNullValue = ((XmlReader) GetNullValue(typeof(XmlReader)));
        private static object _syncLock = new object();
        private static readonly ILog log = LogManager.GetLogger(typeof(TypeHelper));

        static TypeHelper()
        {
            _Init();
        }

        internal static void _Init()
        {
        }

        public static bool CanConvertToChar(object value)
        {
            return ((value is char) || ((value == null) || Convert.CanConvertToChar(value)));
        }

        public static bool CanConvertToNullableChar(object value)
        {
            return ((value is char) || ((value == null) || Convert.CanConvertToNullableChar(value)));
        }

        public static object ChangeType(object value, Type targetType)
        {
            return ConvertChangeType(value, targetType, ReflectionUtils.IsNullable(targetType));
        }

        private static object ConvertChangeType(object value, Type targetType, bool isNullable)
        {
            int num3;
            if (targetType.IsArray)
            {
                if (null == value)
                {
                    return null;
                }
                Type type = value.GetType();
                if (type == targetType)
                {
                    return value;
                }
                if (type.IsArray)
                {
                    Array array2;
                    int length;
                    Type elementType = type.GetElementType();
                    Type type3 = targetType.GetElementType();
                    if ((elementType.IsArray != type3.IsArray) || (elementType.IsArray && (elementType.GetArrayRank() != type3.GetArrayRank())))
                    {
                        throw new InvalidCastException(string.Format("Can not convert array of type '{0}' to array of '{1}'.", type.FullName, targetType.FullName));
                    }
                    Array sourceArray = (Array) value;
                    int rank = sourceArray.Rank;
                    if ((rank == 1) && (0 == sourceArray.GetLowerBound(0)))
                    {
                        length = sourceArray.Length;
                        array2 = Array.CreateInstance(type3, length);
                        if (type3.IsAssignableFrom(elementType))
                        {
                            Array.Copy(sourceArray, array2, length);
                            return array2;
                        }
                        for (num3 = 0; num3 < length; num3++)
                        {
                            array2.SetValue(ConvertChangeType(sourceArray.GetValue(num3), type3, isNullable), num3);
                        }
                        return array2;
                    }
                    length = 1;
                    int[] lengths = new int[rank];
                    int[] indices = new int[rank];
                    int[] lowerBounds = new int[rank];
                    for (num3 = 0; num3 < rank; num3++)
                    {
                        int num6;
                        lengths[num3] = num6 = sourceArray.GetLength(num3);
                        length *= num6;
                        lowerBounds[num3] = sourceArray.GetLowerBound(num3);
                    }
                    array2 = Array.CreateInstance(type3, lengths, lowerBounds);
                    for (num3 = 0; num3 < length; num3++)
                    {
                        int num4 = num3;
                        for (int i = rank - 1; i >= 0; i--)
                        {
                            indices[i] = (num4 % lengths[i]) + lowerBounds[i];
                            num4 /= lengths[i];
                        }
                        array2.SetValue(ConvertChangeType(sourceArray.GetValue(indices), type3, isNullable), indices);
                    }
                    return array2;
                }
            }
            else if (targetType.IsEnum)
            {
                try
                {
                    return Enum.Parse(targetType, value.ToString(), true);
                }
                catch (ArgumentException exception)
                {
                    throw new InvalidCastException(__Res.GetString("TypeHelper_ConversionFail"), exception);
                }
            }
            if (isNullable)
            {
                switch (Type.GetTypeCode(GetUnderlyingType(targetType)))
                {
                    case TypeCode.Boolean:
                        return ConvertToNullableBoolean(value);

                    case TypeCode.Char:
                        return ConvertToNullableChar(value);

                    case TypeCode.SByte:
                        return ConvertToNullableSByte(value);

                    case TypeCode.Byte:
                        return ConvertToNullableByte(value);

                    case TypeCode.Int16:
                        return ConvertToNullableInt16(value);

                    case TypeCode.UInt16:
                        return ConvertToNullableUInt16(value);

                    case TypeCode.Int32:
                        return ConvertToNullableInt32(value);

                    case TypeCode.UInt32:
                        return ConvertToNullableUInt32(value);

                    case TypeCode.Int64:
                        return ConvertToNullableInt64(value);

                    case TypeCode.UInt64:
                        return ConvertToNullableUInt64(value);

                    case TypeCode.Single:
                        return ConvertToNullableSingle(value);

                    case TypeCode.Double:
                        return ConvertToNullableDouble(value);

                    case TypeCode.Decimal:
                        return ConvertToNullableDecimal(value);

                    case TypeCode.DateTime:
                        return ConvertToNullableDateTime(value);
                }
                if (typeof(Guid) == targetType)
                {
                    return ConvertToNullableGuid(value);
                }
            }
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Boolean:
                    return ConvertToBoolean(value);

                case TypeCode.Char:
                    return ConvertToChar(value);

                case TypeCode.SByte:
                    return ConvertToSByte(value);

                case TypeCode.Byte:
                    return ConvertToByte(value);

                case TypeCode.Int16:
                    return ConvertToInt16(value);

                case TypeCode.UInt16:
                    return ConvertToUInt16(value);

                case TypeCode.Int32:
                    return ConvertToInt32(value);

                case TypeCode.UInt32:
                    return ConvertToUInt32(value);

                case TypeCode.Int64:
                    return ConvertToInt64(value);

                case TypeCode.UInt64:
                    return ConvertToUInt64(value);

                case TypeCode.Single:
                    return ConvertToSingle(value);

                case TypeCode.Double:
                    return ConvertToDouble(value);

                case TypeCode.Decimal:
                    return ConvertToDecimal(value);

                case TypeCode.DateTime:
                    return ConvertToDateTime(value);

                case TypeCode.String:
                    return ConvertToString(value);
            }
            if (typeof(Guid) == targetType)
            {
                return ConvertToGuid(value);
            }
            if (typeof(XmlDocument) == targetType)
            {
                return ConvertToXmlDocument(value);
            }
            if (typeof(XDocument) == targetType)
            {
                return ConvertToXDocument(value);
            }
            if (typeof(XElement) == targetType)
            {
                return ConvertToXElement(value);
            }
            if (typeof(byte[]) == targetType)
            {
                return ConvertToByteArray(value);
            }
            if (typeof(char[]) == targetType)
            {
                return ConvertToCharArray(value);
            }
            if (typeof(SqlInt32) == targetType)
            {
                return ConvertToSqlInt32(value);
            }
            if (typeof(SqlString) == targetType)
            {
                return ConvertToSqlString(value);
            }
            if (typeof(SqlDecimal) == targetType)
            {
                return ConvertToSqlDecimal(value);
            }
            if (typeof(SqlDateTime) == targetType)
            {
                return ConvertToSqlDateTime(value);
            }
            if (typeof(SqlBoolean) == targetType)
            {
                return ConvertToSqlBoolean(value);
            }
            if (typeof(SqlMoney) == targetType)
            {
                return ConvertToSqlMoney(value);
            }
            if (typeof(SqlGuid) == targetType)
            {
                return ConvertToSqlGuid(value);
            }
            if (typeof(SqlDouble) == targetType)
            {
                return ConvertToSqlDouble(value);
            }
            if (typeof(SqlByte) == targetType)
            {
                return ConvertToSqlByte(value);
            }
            if (typeof(SqlInt16) == targetType)
            {
                return ConvertToSqlInt16(value);
            }
            if (typeof(SqlInt64) == targetType)
            {
                return ConvertToSqlInt64(value);
            }
            if (typeof(SqlSingle) == targetType)
            {
                return ConvertToSqlSingle(value);
            }
            if (typeof(SqlBinary) == targetType)
            {
                return ConvertToSqlBinary(value);
            }
            if (value != null)
            {
                object obj2;
                Type[] genericArguments;
                MethodInfo method;
                IList list;
                IDictionary dictionary;
                if (targetType.IsAssignableFrom(value.GetType()))
                {
                    return value;
                }
                TypeConverter typeConverter = ReflectionUtils.GetTypeConverter(targetType);
                if ((typeConverter != null) && typeConverter.CanConvertFrom(value.GetType()))
                {
                    return typeConverter.ConvertFrom(value);
                }
                typeConverter = ReflectionUtils.GetTypeConverter(value);
                if ((typeConverter != null) && typeConverter.CanConvertTo(targetType))
                {
                    return typeConverter.ConvertTo(value, targetType);
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.Generic.ICollection`1") && (value is IList))
                {
                    obj2 = CreateInstance(targetType);
                    if (obj2 != null)
                    {
                        genericArguments = ReflectionUtils.GetGenericArguments(targetType);
                        if ((genericArguments != null) && (genericArguments.Length == 1))
                        {
                            Type type4 = targetType.GetInterface("System.Collections.Generic.ICollection`1", true);
                            method = targetType.GetMethod("Add");
                            list = value as IList;
                            for (num3 = 0; num3 < (value as IList).Count; num3++)
                            {
                                method.Invoke(obj2, new object[] { ChangeType(list[num3], genericArguments[0]) });
                            }
                            return obj2;
                        }
                        if (log.get_IsErrorEnabled())
                        {
                            log.Error(string.Format("{0} type arguments of the generic type {1} expecting 1.", genericArguments.Length, targetType.FullName));
                        }
                        return obj2;
                    }
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.IList") && (value is IList))
                {
                    obj2 = CreateInstance(targetType);
                    if (obj2 != null)
                    {
                        list = value as IList;
                        IList list2 = obj2 as IList;
                        for (num3 = 0; num3 < list.Count; num3++)
                        {
                            list2.Add(list[num3]);
                        }
                        return obj2;
                    }
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.Generic.IDictionary`2") && (value is IDictionary))
                {
                    obj2 = CreateInstance(targetType);
                    if (obj2 != null)
                    {
                        dictionary = value as IDictionary;
                        genericArguments = ReflectionUtils.GetGenericArguments(targetType);
                        if ((genericArguments != null) && (genericArguments.Length == 2))
                        {
                            Type type5 = targetType.GetInterface("System.Collections.Generic.IDictionary`2", true);
                            method = targetType.GetMethod("Add");
                            IDictionary dictionary2 = value as IDictionary;
                            foreach (DictionaryEntry entry in dictionary2)
                            {
                                method.Invoke(obj2, new object[] { ChangeType(entry.Key, genericArguments[0]), ChangeType(entry.Value, genericArguments[1]) });
                            }
                            return obj2;
                        }
                        if (log.get_IsErrorEnabled())
                        {
                            log.Error(string.Format("{0} type arguments of the generic type {1} expecting 1.", genericArguments.Length, targetType.FullName));
                        }
                        return obj2;
                    }
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.IDictionary") && (value is IDictionary))
                {
                    obj2 = CreateInstance(targetType);
                    if (obj2 != null)
                    {
                        dictionary = value as IDictionary;
                        IDictionary dictionary3 = obj2 as IDictionary;
                        foreach (DictionaryEntry entry in dictionary)
                        {
                            dictionary3.Add(entry.Key, entry.Value);
                        }
                        return obj2;
                    }
                }
                try
                {
                    return Convert.ChangeType(value, targetType, null);
                }
                catch
                {
                }
            }
            return null;
        }

        public static ASObject ConvertDataSetToASO(DataSet dataSet, bool stronglyTyped)
        {
            ASObject obj2 = new ASObject();
            if (stronglyTyped)
            {
                obj2.TypeName = "DataSet";
            }
            DataTableCollection tables = dataSet.Tables;
            foreach (DataTable table in tables)
            {
                obj2[table.TableName] = ConvertDataTableToASO(table, stronglyTyped);
            }
            return obj2;
        }

        public static ASObject ConvertDataTableToASO(DataTable dataTable, bool stronglyTyped)
        {
            int num;
            if (dataTable.ExtendedProperties.Contains("DynamicPage"))
            {
                return ConvertPageableDataTableToASO(dataTable, stronglyTyped);
            }
            ASObject obj2 = new ASObject();
            if (stronglyTyped)
            {
                obj2.TypeName = "RecordSet";
            }
            ASObject obj3 = new ASObject();
            if (dataTable.ExtendedProperties["TotalCount"] != null)
            {
                obj3["totalCount"] = (int) dataTable.ExtendedProperties["TotalCount"];
            }
            else
            {
                obj3["totalCount"] = dataTable.Rows.Count;
            }
            if (dataTable.ExtendedProperties["Service"] != null)
            {
                obj3["serviceName"] = "rs://" + dataTable.ExtendedProperties["Service"];
            }
            else
            {
                obj3["serviceName"] = "FluorineFx.PageableResult";
            }
            obj3["version"] = 1;
            obj3["cursor"] = 1;
            if (dataTable.ExtendedProperties["RecordsetId"] != null)
            {
                obj3["id"] = dataTable.ExtendedProperties["RecordsetId"] as string;
            }
            else
            {
                obj3["id"] = null;
            }
            string[] strArray = new string[dataTable.Columns.Count];
            for (num = 0; num < dataTable.Columns.Count; num++)
            {
                strArray[num] = dataTable.Columns[num].ColumnName;
            }
            obj3["columnNames"] = strArray;
            object[] objArray = new object[dataTable.Rows.Count];
            for (num = 0; num < dataTable.Rows.Count; num++)
            {
                objArray[num] = dataTable.Rows[num].ItemArray;
            }
            obj3["initialData"] = objArray;
            obj2["serverInfo"] = obj3;
            return obj2;
        }

        public static ASObject ConvertPageableDataTableToASO(DataTable dataTable, bool stronglyTyped)
        {
            ASObject obj2 = new ASObject();
            if (stronglyTyped)
            {
                obj2.TypeName = "RecordSetPage";
            }
            obj2["Cursor"] = (int) dataTable.ExtendedProperties["Cursor"];
            ArrayList list = new ArrayList();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                list.Add(dataTable.Rows[i].ItemArray);
            }
            obj2["Page"] = list;
            return obj2;
        }

        public static bool ConvertToBoolean(object value)
        {
            return ((value is bool) ? ((bool) value) : ((value == null) ? _defaultBooleanNullValue : Convert.ToBoolean(value)));
        }

        public static byte ConvertToByte(object value)
        {
            return ((value is byte) ? ((byte) value) : ((value == null) ? _defaultByteNullValue : Convert.ToByte(value)));
        }

        public static byte[] ConvertToByteArray(object value)
        {
            return ((value is byte[]) ? ((byte[]) value) : ((value == null) ? null : Convert.ToByteArray(value)));
        }

        public static char ConvertToChar(object value)
        {
            return ((value is char) ? ((char) value) : ((value == null) ? _defaultCharNullValue : Convert.ToChar(value)));
        }

        public static char[] ConvertToCharArray(object value)
        {
            return ((value is char[]) ? ((char[]) value) : ((value == null) ? null : Convert.ToCharArray(value)));
        }

        public static DateTime ConvertToDateTime(object value)
        {
            return ((value is DateTime) ? ((DateTime) value) : ((value == null) ? _defaultDateTimeNullValue : Convert.ToDateTime(value)));
        }

        public static decimal ConvertToDecimal(object value)
        {
            return ((value is decimal) ? ((decimal) value) : ((value == null) ? _defaultDecimalNullValue : Convert.ToDecimal(value)));
        }

        public static double ConvertToDouble(object value)
        {
            return ((value is double) ? ((double) value) : ((value == null) ? _defaultDoubleNullValue : Convert.ToDouble(value)));
        }

        public static Guid ConvertToGuid(object value)
        {
            return ((value is Guid) ? ((Guid) value) : ((value == null) ? _defaultGuidNullValue : Convert.ToGuid(value)));
        }

        public static short ConvertToInt16(object value)
        {
            return ((value is short) ? ((short) value) : ((value == null) ? _defaultInt16NullValue : Convert.ToInt16(value)));
        }

        public static int ConvertToInt32(object value)
        {
            return ((value is int) ? ((int) value) : ((value == null) ? _defaultInt32NullValue : Convert.ToInt32(value)));
        }

        public static long ConvertToInt64(object value)
        {
            return ((value is long) ? ((long) value) : ((value == null) ? _defaultInt64NullValue : Convert.ToInt64(value)));
        }

        public static bool? ConvertToNullableBoolean(object value)
        {
            if (value is bool)
            {
                return (bool?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableBoolean(value);
        }

        public static byte? ConvertToNullableByte(object value)
        {
            if (value is byte)
            {
                return (byte?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableByte(value);
        }

        public static char? ConvertToNullableChar(object value)
        {
            if (value is char)
            {
                return (char?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableChar(value);
        }

        public static DateTime? ConvertToNullableDateTime(object value)
        {
            if (value is DateTime)
            {
                return (DateTime?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableDateTime(value);
        }

        public static decimal? ConvertToNullableDecimal(object value)
        {
            if (value is decimal)
            {
                return (decimal?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableDecimal(value);
        }

        public static double? ConvertToNullableDouble(object value)
        {
            if (value is double)
            {
                return (double?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableDouble(value);
        }

        public static Guid? ConvertToNullableGuid(object value)
        {
            if (value is Guid)
            {
                return (Guid?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableGuid(value);
        }

        public static short? ConvertToNullableInt16(object value)
        {
            if (value is short)
            {
                return (short?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableInt16(value);
        }

        public static int? ConvertToNullableInt32(object value)
        {
            if (value is int)
            {
                return (int?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableInt32(value);
        }

        public static long? ConvertToNullableInt64(object value)
        {
            if (value is long)
            {
                return (long?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableInt64(value);
        }

        [CLSCompliant(false)]
        public static sbyte? ConvertToNullableSByte(object value)
        {
            if (value is sbyte)
            {
                return (sbyte?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableSByte(value);
        }

        public static float? ConvertToNullableSingle(object value)
        {
            if (value is float)
            {
                return (float?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableSingle(value);
        }

        [CLSCompliant(false)]
        public static ushort? ConvertToNullableUInt16(object value)
        {
            if (value is ushort)
            {
                return (ushort?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableUInt16(value);
        }

        [CLSCompliant(false)]
        public static uint? ConvertToNullableUInt32(object value)
        {
            if (value is uint)
            {
                return (uint?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableUInt32(value);
        }

        [CLSCompliant(false)]
        public static ulong? ConvertToNullableUInt64(object value)
        {
            if (value is ulong)
            {
                return (ulong?) value;
            }
            if (value == null)
            {
                return null;
            }
            return Convert.ToNullableUInt64(value);
        }

        [CLSCompliant(false)]
        public static sbyte ConvertToSByte(object value)
        {
            return ((value is sbyte) ? ((sbyte) value) : ((value == null) ? _defaultSByteNullValue : Convert.ToSByte(value)));
        }

        public static float ConvertToSingle(object value)
        {
            return ((value is float) ? ((float) value) : ((value == null) ? _defaultSingleNullValue : Convert.ToSingle(value)));
        }

        public static SqlBinary ConvertToSqlBinary(object value)
        {
            return ((value == null) ? SqlBinary.Null : ((value is SqlBinary) ? ((SqlBinary) value) : Convert.ToSqlBinary(value)));
        }

        public static SqlBoolean ConvertToSqlBoolean(object value)
        {
            return ((value == null) ? SqlBoolean.Null : ((value is SqlBoolean) ? ((SqlBoolean) value) : Convert.ToSqlBoolean(value)));
        }

        public static SqlByte ConvertToSqlByte(object value)
        {
            return ((value == null) ? SqlByte.Null : ((value is SqlByte) ? ((SqlByte) value) : Convert.ToSqlByte(value)));
        }

        public static SqlDateTime ConvertToSqlDateTime(object value)
        {
            return ((value == null) ? SqlDateTime.Null : ((value is SqlDateTime) ? ((SqlDateTime) value) : Convert.ToSqlDateTime(value)));
        }

        public static SqlDecimal ConvertToSqlDecimal(object value)
        {
            return ((value == null) ? SqlDecimal.Null : ((value is SqlDecimal) ? ((SqlDecimal) value) : ((value is SqlMoney) ? ((SqlMoney) value).ToSqlDecimal() : Convert.ToSqlDecimal(value))));
        }

        public static SqlDouble ConvertToSqlDouble(object value)
        {
            return ((value == null) ? SqlDouble.Null : ((value is SqlDouble) ? ((SqlDouble) value) : Convert.ToSqlDouble(value)));
        }

        public static SqlGuid ConvertToSqlGuid(object value)
        {
            return ((value == null) ? SqlGuid.Null : ((value is SqlGuid) ? ((SqlGuid) value) : Convert.ToSqlGuid(value)));
        }

        public static SqlInt16 ConvertToSqlInt16(object value)
        {
            return ((value == null) ? SqlInt16.Null : ((value is SqlInt16) ? ((SqlInt16) value) : Convert.ToSqlInt16(value)));
        }

        public static SqlInt32 ConvertToSqlInt32(object value)
        {
            return ((value == null) ? SqlInt32.Null : ((value is SqlInt32) ? ((SqlInt32) value) : Convert.ToSqlInt32(value)));
        }

        public static SqlInt64 ConvertToSqlInt64(object value)
        {
            return ((value == null) ? SqlInt64.Null : ((value is SqlInt64) ? ((SqlInt64) value) : Convert.ToSqlInt64(value)));
        }

        public static SqlMoney ConvertToSqlMoney(object value)
        {
            return ((value == null) ? SqlMoney.Null : ((value is SqlMoney) ? ((SqlMoney) value) : ((value is SqlDecimal) ? ((SqlDecimal) value).ToSqlMoney() : Convert.ToSqlMoney(value))));
        }

        public static SqlSingle ConvertToSqlSingle(object value)
        {
            return ((value == null) ? SqlSingle.Null : ((value is SqlSingle) ? ((SqlSingle) value) : Convert.ToSqlSingle(value)));
        }

        public static SqlString ConvertToSqlString(object value)
        {
            return ((value == null) ? SqlString.Null : ((value is SqlString) ? ((SqlString) value) : Convert.ToSqlString(value)));
        }

        public static string ConvertToString(object value)
        {
            return ((value is string) ? ((string) value) : ((value == null) ? _defaultStringNullValue : Convert.ToString(value)));
        }

        [CLSCompliant(false)]
        public static ushort ConvertToUInt16(object value)
        {
            return ((value is ushort) ? ((ushort) value) : ((value == null) ? _defaultUInt16NullValue : Convert.ToUInt16(value)));
        }

        [CLSCompliant(false)]
        public static uint ConvertToUInt32(object value)
        {
            return ((value is uint) ? ((uint) value) : ((value == null) ? _defaultUInt32NullValue : Convert.ToUInt32(value)));
        }

        [CLSCompliant(false)]
        public static ulong ConvertToUInt64(object value)
        {
            return ((value is ulong) ? ((ulong) value) : ((value == null) ? _defaultUInt64NullValue : Convert.ToUInt64(value)));
        }

        public static XDocument ConvertToXDocument(object value)
        {
            return ((value is XDocument) ? ((XDocument) value) : ((value == null) ? _defaultXDocumentNullValue : Convert.ToXDocument(value)));
        }

        public static XElement ConvertToXElement(object value)
        {
            return ((value is XElement) ? ((XElement) value) : ((value == null) ? _defaultXElementNullValue : Convert.ToXElement(value)));
        }

        public static XmlDocument ConvertToXmlDocument(object value)
        {
            return ((value is XmlDocument) ? ((XmlDocument) value) : ((value == null) ? _defaultXmlDocumentNullValue : Convert.ToXmlDocument(value)));
        }

        public static XmlReader ConvertToXmlReader(object value)
        {
            return ((value is XmlReader) ? ((XmlReader) value) : ((value == null) ? _defaultXmlReaderNullValue : Convert.ToXmlReader(value)));
        }

        internal static object CreateInstance(Type type)
        {
            if (ReflectionUtils.IsGenericType(type))
            {
                Type genericTypeDefinition = ReflectionUtils.GetGenericTypeDefinition(type);
                Type[] genericArguments = ReflectionUtils.GetGenericArguments(type);
                object obj2 = Activator.CreateInstance(ReflectionUtils.MakeGenericType(genericTypeDefinition, genericArguments));
                if ((obj2 == null) && ((log != null) && log.get_IsErrorEnabled()))
                {
                    string str = string.Format("Could not instantiate the generic type {0}.", type.FullName);
                    log.Error(str);
                }
                return obj2;
            }
            return Activator.CreateInstance(type);
        }

        public static Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public static string GetCSharpName(Type type)
        {
            int num = 0;
            while (type.IsArray)
            {
                type = type.GetElementType();
                num++;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(type.Namespace);
            sb.Append(".");
            Type[] emptyTypes = Type.EmptyTypes;
            if (ReflectionUtils.IsGenericType(type) && (ReflectionUtils.GetGenericArguments(type) != null))
            {
                emptyTypes = ReflectionUtils.GetGenericArguments(type);
            }
            GetCSharpName(type, emptyTypes, 0, sb);
            for (int i = 0; i < num; i++)
            {
                sb.Append("[]");
            }
            return sb.ToString();
        }

        private static int GetCSharpName(Type type, Type[] parameters, int index, StringBuilder sb)
        {
            if ((type.DeclaringType != null) && (type.DeclaringType != type))
            {
                index = GetCSharpName(type.DeclaringType, parameters, index, sb);
                sb.Append(".");
            }
            string name = type.Name;
            int length = name.IndexOf('`');
            if (length < 0)
            {
                length = name.IndexOf('!');
            }
            if (length > 0)
            {
                sb.Append(name.Substring(0, length));
                sb.Append("<");
                int num2 = int.Parse(name.Substring(length + 1), CultureInfo.InvariantCulture) + index;
                while (index < num2)
                {
                    sb.Append(GetCSharpName(parameters[index]));
                    if (index < (num2 - 1))
                    {
                        sb.Append(",");
                    }
                    index++;
                }
                sb.Append(">");
                return index;
            }
            sb.Append(name);
            return index;
        }

        public static string GetDescription(MethodInfo methodInfo)
        {
            Attribute attribute = ReflectionUtils.GetAttribute(typeof(DescriptionAttribute), methodInfo, false);
            if (attribute != null)
            {
                return (attribute as DescriptionAttribute).Description;
            }
            return null;
        }

        public static string GetDescription(Type type)
        {
            Attribute attribute = ReflectionUtils.GetAttribute(typeof(DescriptionAttribute), type, false);
            if (attribute != null)
            {
                return (attribute as DescriptionAttribute).Description;
            }
            return null;
        }

        public static string[] GetLacLocations()
        {
            ArrayList list = new ArrayList();
            try
            {
                log.Debug("Checking FluorineFx location");
                try
                {
                    string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (directoryName != null)
                    {
                        list.Add(directoryName);
                        log.Debug(string.Format("Adding LAC location {0}", directoryName));
                    }
                }
                catch (SecurityException)
                {
                }
                try
                {
                    if (IsMono)
                    {
                        log.Debug("Checking Mono DynamicBase");
                        if (AppDomain.CurrentDomain.SetupInformation.DynamicBase != null)
                        {
                            string str2 = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.DynamicBase);
                            list.Add(str2);
                            log.Debug(string.Format("Adding LAC location {0}", str2));
                        }
                    }
                    else
                    {
                        log.Debug("Checking DynamicDirectory");
                        if (AppDomain.CurrentDomain.DynamicDirectory != null)
                        {
                            string str3 = Path.GetDirectoryName(AppDomain.CurrentDomain.DynamicDirectory);
                            list.Add(str3);
                            log.Debug(string.Format("Adding LAC location {0}", str3));
                        }
                    }
                }
                catch (SecurityException)
                {
                }
                try
                {
                    if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
                    {
                        log.Debug("Checking Request PhysicalApplicationPath");
                        string str4 = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "bin");
                        list.Add(str4);
                        log.Debug(string.Format("Adding LAC location {0}", str4));
                        str4 = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Bin");
                        list.Add(str4);
                        log.Debug(string.Format("Adding LAC location {0}", str4));
                    }
                }
                catch (SecurityException)
                {
                }
            }
            catch (Exception exception)
            {
                log.Error("An error occurred while configuring LAC locations. This may lead to assembly load failures.", exception);
            }
            return (list.ToArray(typeof(string)) as string[]);
        }

        internal static object GetNullValue(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if ((FluorineConfiguration.Instance.NullableValues != null) && FluorineConfiguration.Instance.NullableValues.ContainsKey(type))
            {
                return FluorineConfiguration.Instance.NullableValues[type];
            }
            if (type.IsValueType)
            {
                if (type.IsPrimitive)
                {
                    if (type == typeof(int))
                    {
                        return 0;
                    }
                    if (type == typeof(double))
                    {
                        return 0.0;
                    }
                    if (type == typeof(short))
                    {
                        return (short) 0;
                    }
                    if (type == typeof(bool))
                    {
                        return false;
                    }
                    if (type == typeof(sbyte))
                    {
                        return (sbyte) 0;
                    }
                    if (type == typeof(long))
                    {
                        return 0L;
                    }
                    if (type == typeof(byte))
                    {
                        return (byte) 0;
                    }
                    if (type == typeof(ushort))
                    {
                        return (ushort) 0;
                    }
                    if (type == typeof(uint))
                    {
                        return 0;
                    }
                    if (type == typeof(ulong))
                    {
                        return (ulong) 0L;
                    }
                    if (type == typeof(float))
                    {
                        return 0f;
                    }
                    if (type == typeof(char))
                    {
                        return '\0';
                    }
                }
                else
                {
                    if (type == typeof(DateTime))
                    {
                        return DateTime.MinValue;
                    }
                    if (type == typeof(decimal))
                    {
                        return 0M;
                    }
                    if (type == typeof(Guid))
                    {
                        return Guid.Empty;
                    }
                    if (type == typeof(SqlInt32))
                    {
                        return SqlInt32.Null;
                    }
                    if (type == typeof(SqlString))
                    {
                        return SqlString.Null;
                    }
                    if (type == typeof(SqlBoolean))
                    {
                        return SqlBoolean.Null;
                    }
                    if (type == typeof(SqlByte))
                    {
                        return SqlByte.Null;
                    }
                    if (type == typeof(SqlDateTime))
                    {
                        return SqlDateTime.Null;
                    }
                    if (type == typeof(SqlDecimal))
                    {
                        return SqlDecimal.Null;
                    }
                    if (type == typeof(SqlDouble))
                    {
                        return SqlDouble.Null;
                    }
                    if (type == typeof(SqlGuid))
                    {
                        return SqlGuid.Null;
                    }
                    if (type == typeof(SqlInt16))
                    {
                        return SqlInt16.Null;
                    }
                    if (type == typeof(SqlInt64))
                    {
                        return SqlInt64.Null;
                    }
                    if (type == typeof(SqlMoney))
                    {
                        return SqlMoney.Null;
                    }
                    if (type == typeof(SqlSingle))
                    {
                        return SqlSingle.Null;
                    }
                    if (type == typeof(SqlBinary))
                    {
                        return SqlBinary.Null;
                    }
                }
            }
            else
            {
                if (type == typeof(string))
                {
                    return null;
                }
                if (type == typeof(DBNull))
                {
                    return DBNull.Value;
                }
            }
            return null;
        }

        public static bool GetTypeIsAccessible(Type type)
        {
            if (type == null)
            {
                return false;
            }
            if (type.Assembly == typeof(TypeHelper).Assembly)
            {
                return false;
            }
            if (FluorineConfiguration.Instance.RemotingServiceAttributeConstraint == RemotingServiceAttributeConstraint.Access)
            {
                return (ReflectionUtils.GetAttribute(typeof(RemotingServiceAttribute), type, false) != null);
            }
            return true;
        }

        public static Type GetUnderlyingType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (ReflectionUtils.IsNullable(type))
            {
                type = type.GetGenericArguments()[0];
            }
            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }
            return type;
        }

        public static bool IsAssignable(object obj, Type targetType)
        {
            return IsAssignable(obj, targetType, ReflectionUtils.IsNullable(targetType));
        }

        private static bool IsAssignable(object obj, Type targetType, bool isNullable)
        {
            if ((obj != null) && targetType.IsAssignableFrom(obj.GetType()))
            {
                return true;
            }
            if (isNullable && (obj == null))
            {
                return true;
            }
            if (targetType.IsArray)
            {
                if (null == obj)
                {
                    return true;
                }
                Type type = obj.GetType();
                if (type == targetType)
                {
                    return true;
                }
                if (type.IsArray)
                {
                    int length;
                    int num3;
                    Type elementType = type.GetElementType();
                    Type type3 = targetType.GetElementType();
                    if ((elementType.IsArray != type3.IsArray) || (elementType.IsArray && (elementType.GetArrayRank() != type3.GetArrayRank())))
                    {
                        return false;
                    }
                    Array array = (Array) obj;
                    int rank = array.Rank;
                    if ((rank == 1) && (0 == array.GetLowerBound(0)))
                    {
                        length = array.Length;
                        if (type3.IsAssignableFrom(elementType))
                        {
                            return true;
                        }
                        for (num3 = 0; num3 < length; num3++)
                        {
                            if (!IsAssignable(array.GetValue(num3), type3))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        length = 1;
                        int[] numArray = new int[rank];
                        int[] indices = new int[rank];
                        int[] numArray3 = new int[rank];
                        for (num3 = 0; num3 < rank; num3++)
                        {
                            int num6;
                            numArray[num3] = num6 = array.GetLength(num3);
                            length *= num6;
                            numArray3[num3] = array.GetLowerBound(num3);
                        }
                        for (num3 = 0; num3 < length; num3++)
                        {
                            int num4 = num3;
                            for (int i = rank - 1; i >= 0; i--)
                            {
                                indices[i] = (num4 % numArray[i]) + numArray3[i];
                                num4 /= numArray[i];
                            }
                            if (!IsAssignable(array.GetValue(indices), type3))
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            else if (targetType.IsEnum)
            {
                try
                {
                    Enum.Parse(targetType, obj.ToString(), true);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            if (obj != null)
            {
                Type[] genericArguments;
                TypeConverter typeConverter = ReflectionUtils.GetTypeConverter(obj);
                if ((typeConverter != null) && typeConverter.CanConvertTo(targetType))
                {
                    return true;
                }
                typeConverter = ReflectionUtils.GetTypeConverter(targetType);
                if ((typeConverter != null) && typeConverter.CanConvertFrom(obj.GetType()))
                {
                    return true;
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.Generic.ICollection`1") && (obj is IList))
                {
                    genericArguments = ReflectionUtils.GetGenericArguments(targetType);
                    return (((genericArguments != null) && (genericArguments.Length == 1)) && (targetType.GetInterface("System.Collections.Generic.ICollection`1", true) != null));
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.IList") && (obj is IList))
                {
                    return true;
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.Generic.IDictionary`2") && (obj is IDictionary))
                {
                    genericArguments = ReflectionUtils.GetGenericArguments(targetType);
                    return (((genericArguments != null) && (genericArguments.Length == 2)) && (targetType.GetInterface("System.Collections.Generic.IDictionary`2", true) != null));
                }
                if (ReflectionUtils.ImplementsInterface(targetType, "System.Collections.IDictionary") && (obj is IDictionary))
                {
                    return true;
                }
            }
            else
            {
                if (!(targetType is INullable) && targetType.IsValueType)
                {
                    return FluorineConfiguration.Instance.AcceptNullValueTypes;
                }
                return true;
            }
            try
            {
                if (isNullable && (Type.GetTypeCode(GetUnderlyingType(targetType)) == TypeCode.Char))
                {
                    return CanConvertToNullableChar(obj);
                }
                if (Type.GetTypeCode(targetType) == TypeCode.Char)
                {
                    return CanConvertToChar(obj);
                }
            }
            catch (InvalidCastException)
            {
            }
            return (((typeof(XDocument) == targetType) && (obj is XmlDocument)) || ((typeof(XElement) == targetType) && (obj is XmlDocument)));
        }

        public static Type Locate(string typeName)
        {
            if ((typeName != null) && (typeName != string.Empty))
            {
                foreach (Assembly assembly in GetAssemblies())
                {
                    Type type = assembly.GetType(typeName, false);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        public static Type LocateInLac(string typeName, string lac)
        {
            if (lac != null)
            {
                Type type;
                if ((typeName == null) || (typeName == string.Empty))
                {
                    return null;
                }
                foreach (string str in Directory.GetFiles(lac, "*.dll"))
                {
                    try
                    {
                        log.Debug(__Res.GetString("TypeHelper_Probing", new object[] { str }));
                        type = Assembly.LoadFrom(str).GetType(typeName, false);
                        if (type != null)
                        {
                            return type;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (log.get_IsWarnEnabled())
                        {
                            log.Warn(__Res.GetString("TypeHelper_LoadDllFail", new object[] { str }));
                            log.Warn(exception.Message);
                        }
                    }
                }
                foreach (string str2 in Directory.GetDirectories(lac))
                {
                    type = LocateInLac(typeName, str2);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        internal static void NarrowValues(object[] values, ParameterInfo[] parameterInfos)
        {
            for (int i = 0; (values != null) && (i < values.Length); i++)
            {
                object obj2 = values[i];
                values[i] = ChangeType(obj2, parameterInfos[i].ParameterType);
            }
        }

        public static Type[] SearchAllTypes(string lac, Hashtable excludedBaseTypes)
        {
            ArrayList list = new ArrayList();
            foreach (string str in Directory.GetFiles(lac, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(str);
                    if (assembly != Assembly.GetExecutingAssembly())
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            if ((excludedBaseTypes == null) || (!excludedBaseTypes.ContainsKey(type) && ((type.BaseType == null) || !excludedBaseTypes.ContainsKey(type.BaseType))))
                            {
                                list.Add(type);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (log.get_IsWarnEnabled())
                    {
                        log.Warn(__Res.GetString("TypeHelper_LoadDllFail", new object[] { str }));
                        log.Warn(exception.Message);
                    }
                }
            }
            return (Type[]) list.ToArray(typeof(Type));
        }

        public static bool SkipMethod(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType == typeof(IAsyncResult))
            {
                return true;
            }
            foreach (ParameterInfo info in methodInfo.GetParameters())
            {
                if (info.ParameterType == typeof(IAsyncResult))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsMono
        {
            get
            {
                return (typeof(object).Assembly.GetType("System.MonoType") != null);
            }
        }
    }
}

