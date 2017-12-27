namespace FluorineFx.Json
{
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class JsonSerializer
    {
        private JsonConverterCollection _converters;
        private int _level;
        private FluorineFx.Json.ReferenceLoopHandling _referenceLoopHandling = FluorineFx.Json.ReferenceLoopHandling.Error;
        private Hashtable _typeMemberMappings;

        private MemberMappingCollection CreateMemberMappings(Type objectType)
        {
            MemberInfo[] fieldsAndProperties = ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Public | BindingFlags.Instance);
            MemberMappingCollection mappings = new MemberMappingCollection();
            foreach (MemberInfo info in fieldsAndProperties)
            {
                string propertyName;
                JsonPropertyAttribute attribute = ReflectionUtils.GetAttribute(typeof(JsonPropertyAttribute), info, true) as JsonPropertyAttribute;
                if (attribute != null)
                {
                    propertyName = attribute.PropertyName;
                }
                else
                {
                    propertyName = info.Name;
                }
                bool ignored = info.IsDefined(typeof(JsonIgnoreAttribute), true);
                bool readable = ReflectionUtils.CanReadMemberValue(info);
                bool writable = ReflectionUtils.CanSetMemberValue(info);
                MemberMapping memberMapping = new MemberMapping(propertyName, info, ignored, readable, writable);
                mappings.Add(memberMapping);
            }
            return mappings;
        }

        public object Deserialize(JsonReader reader)
        {
            return this.Deserialize(reader, null);
        }

        public object Deserialize(JsonReader reader, Type objectType)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (!reader.Read())
            {
                return null;
            }
            return this.GetObject(reader, objectType);
        }

        private object EnsureType(object value, Type targetType)
        {
            if (value == null)
            {
                return null;
            }
            if (targetType == null)
            {
                return value;
            }
            Type sourceType = value.GetType();
            if (sourceType == targetType)
            {
                return value;
            }
            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            if (!converter.CanConvertFrom(sourceType))
            {
                if (converter.CanConvertFrom(typeof(string)))
                {
                    string text = TypeDescriptor.GetConverter(value).ConvertToInvariantString(value);
                    return converter.ConvertFromInvariantString(text);
                }
                if (!targetType.IsAssignableFrom(sourceType))
                {
                    throw new InvalidOperationException(string.Format("Cannot convert object of type '{0}' to type '{1}'", value.GetType(), targetType));
                }
                return value;
            }
            return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        }

        private MemberMappingCollection GetMemberMappings(Type objectType)
        {
            if (this._typeMemberMappings == null)
            {
                this._typeMemberMappings = new Hashtable();
            }
            if (this._typeMemberMappings.Contains(objectType))
            {
                return (this._typeMemberMappings[objectType] as MemberMappingCollection);
            }
            MemberMappingCollection mappings = this.CreateMemberMappings(objectType);
            this._typeMemberMappings[objectType] = mappings;
            return mappings;
        }

        private object GetObject(JsonReader reader, Type objectType)
        {
            object obj2;
            JsonConverter converter;
            this._level++;
            if (this.HasMatchingConverter(objectType, out converter))
            {
                return converter.ReadJson(reader, objectType);
            }
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    obj2 = (objectType != null) ? this.PopulateObject(reader, objectType) : this.PopulateJavaScriptObject(reader);
                    break;

                case JsonToken.StartArray:
                    obj2 = (objectType != null) ? this.PopulateList(reader, objectType) : this.PopulateJavaScriptArray(reader);
                    break;

                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Date:
                    obj2 = this.EnsureType(reader.Value, objectType);
                    break;

                case JsonToken.Null:
                case JsonToken.Undefined:
                    obj2 = null;
                    break;

                case JsonToken.Constructor:
                    obj2 = reader.Value.ToString();
                    break;

                default:
                    throw new JsonSerializationException("Unexpected token whil deserializing object: " + reader.TokenType);
            }
            this._level--;
            return obj2;
        }

        private bool HasMatchingConverter(Type type, out JsonConverter matchingConverter)
        {
            if (this._converters != null)
            {
                for (int i = 0; i < this._converters.Count; i++)
                {
                    JsonConverter converter = this._converters[i];
                    if (converter.CanConvert(type))
                    {
                        matchingConverter = converter;
                        return true;
                    }
                }
            }
            matchingConverter = null;
            return false;
        }

        private JavaScriptArray PopulateJavaScriptArray(JsonReader reader)
        {
            JavaScriptArray array = new JavaScriptArray();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndArray:
                        return array;

                    default:
                    {
                        object obj2 = this.GetObject(reader, null);
                        array.Add(obj2);
                        break;
                    }
                }
            }
            throw new JsonSerializationException("Unexpected end while deserializing array.");
        }

        private JavaScriptObject PopulateJavaScriptObject(JsonReader reader)
        {
            JavaScriptObject obj2 = new JavaScriptObject();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                    {
                        string str = reader.Value.ToString();
                        do
                        {
                            if (!reader.Read())
                            {
                                throw new JsonSerializationException("Unexpected end while deserializing object.");
                            }
                        }
                        while (reader.TokenType == JsonToken.Comment);
                        object obj3 = this.GetObject(reader, null);
                        obj2[str] = obj3;
                        break;
                    }
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndObject:
                        return obj2;

                    default:
                        throw new JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
                }
            }
            throw new JsonSerializationException("Unexpected end while deserializing object.");
        }

        private object PopulateList(JsonReader reader, Type objectType)
        {
            Type listItemType = ReflectionUtils.GetListItemType(objectType);
            IList list = CollectionUtils.CreateList(objectType);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndArray:
                        return list;

                    default:
                    {
                        object obj2 = this.GetObject(reader, listItemType);
                        list.Add(obj2);
                        break;
                    }
                }
            }
            throw new JsonSerializationException("Unexpected end when deserializing array.");
        }

        private object PopulateObject(JsonReader reader, Type objectType)
        {
            object target = Activator.CreateInstance(objectType);
            while (reader.Read())
            {
                JsonToken tokenType = reader.TokenType;
                if (tokenType != JsonToken.PropertyName)
                {
                    if (tokenType != JsonToken.EndObject)
                    {
                        throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
                    }
                    return target;
                }
                string memberName = reader.Value.ToString();
                this.SetObjectMember(reader, target, objectType, memberName);
            }
            throw new JsonSerializationException("Unexpected end when deserializing object.");
        }

        public void Serialize(JsonWriter jsonWriter, object value)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException("jsonWriter");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.SerializeValue(jsonWriter, value);
        }

        public void Serialize(TextWriter textWriter, object value)
        {
            this.Serialize(new JsonWriter(textWriter), value);
        }

        private void SerializeCollection(JsonWriter writer, ICollection values)
        {
            object[] array = new object[values.Count];
            values.CopyTo(array, 0);
            this.SerializeList(writer, array);
        }

        private void SerializeDictionary(JsonWriter writer, IDictionary values)
        {
            writer.WriteStartObject();
            foreach (DictionaryEntry entry in values)
            {
                writer.WritePropertyName(entry.Key.ToString());
                this.SerializeValue(writer, entry.Value);
            }
            writer.WriteEndObject();
        }

        private void SerializeList(JsonWriter writer, IList values)
        {
            writer.WriteStartArray();
            for (int i = 0; i < values.Count; i++)
            {
                this.SerializeValue(writer, values[i]);
            }
            writer.WriteEndArray();
        }

        private void SerializeObject(JsonWriter writer, object value)
        {
            Type type = value.GetType();
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if ((((converter != null) && !(converter is ComponentConverter)) && (converter.GetType() != typeof(TypeConverter))) && converter.CanConvertTo(typeof(string)))
            {
                writer.WriteValue(converter.ConvertToInvariantString(value));
            }
            else
            {
                writer.SerializeStack.Add(value);
                writer.WriteStartObject();
                MemberMappingCollection memberMappings = this.GetMemberMappings(type);
                foreach (MemberMapping mapping in memberMappings)
                {
                    if (!(mapping.Ignored || !mapping.Readable))
                    {
                        this.WriteMemberInfoProperty(writer, value, mapping.Member, mapping.MappingName);
                    }
                }
                writer.WriteEndObject();
                writer.SerializeStack.Remove(value);
            }
        }

        private void SerializeValue(JsonWriter writer, object value)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                JsonConverter converter;
                if (this.HasMatchingConverter(value.GetType(), out converter))
                {
                    converter.WriteJson(writer, value);
                }
                else if (!(value is IConvertible))
                {
                    if (value is IList)
                    {
                        this.SerializeList(writer, (IList) value);
                    }
                    else if (value is IDictionary)
                    {
                        this.SerializeDictionary(writer, (IDictionary) value);
                    }
                    else if (value is ICollection)
                    {
                        this.SerializeCollection(writer, (ICollection) value);
                    }
                    else if (value is Identifier)
                    {
                        writer.WriteRaw(value.ToString());
                    }
                    else
                    {
                        this.SerializeObject(writer, value);
                    }
                }
                else
                {
                    IConvertible convertible = value as IConvertible;
                    switch (convertible.GetTypeCode())
                    {
                        case TypeCode.Boolean:
                            writer.WriteValue((bool) convertible);
                            return;

                        case TypeCode.Char:
                            writer.WriteValue((char) convertible);
                            return;

                        case TypeCode.SByte:
                            writer.WriteValue((sbyte) convertible);
                            return;

                        case TypeCode.Byte:
                            writer.WriteValue((byte) convertible);
                            return;

                        case TypeCode.Int16:
                            writer.WriteValue((short) convertible);
                            return;

                        case TypeCode.UInt16:
                            writer.WriteValue((ushort) convertible);
                            return;

                        case TypeCode.Int32:
                            writer.WriteValue((int) convertible);
                            return;

                        case TypeCode.UInt32:
                            writer.WriteValue((uint) convertible);
                            return;

                        case TypeCode.Int64:
                            writer.WriteValue((long) convertible);
                            return;

                        case TypeCode.UInt64:
                            writer.WriteValue((ulong) convertible);
                            return;

                        case TypeCode.Single:
                            writer.WriteValue((float) convertible);
                            return;

                        case TypeCode.Double:
                            writer.WriteValue((double) convertible);
                            return;

                        case TypeCode.Decimal:
                            writer.WriteValue((decimal) convertible);
                            return;

                        case TypeCode.DateTime:
                            writer.WriteValue((DateTime) convertible);
                            return;

                        case TypeCode.String:
                            writer.WriteValue((string) convertible);
                            return;
                    }
                    this.SerializeObject(writer, value);
                }
            }
        }

        private void SetObjectMember(JsonReader reader, object target, Type targetType, string memberName)
        {
            Type memberUnderlyingType;
            object obj2;
            if (!reader.Read())
            {
                throw new JsonSerializationException(string.Format("Unexpected end when setting {0}'s value.", memberName));
            }
            MemberMappingCollection memberMappings = this.GetMemberMappings(targetType);
            if (memberMappings.Contains(memberName))
            {
                MemberMapping mapping = memberMappings[memberName];
                if (!mapping.Ignored && mapping.Writable)
                {
                    memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(mapping.Member);
                    obj2 = this.GetObject(reader, memberUnderlyingType);
                    ReflectionUtils.SetMemberValue(mapping.Member, target, obj2);
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(targetType))
            {
                memberUnderlyingType = ReflectionUtils.GetDictionaryValueType(target.GetType());
                obj2 = this.GetObject(reader, memberUnderlyingType);
                ((IDictionary) target).Add(memberName, obj2);
            }
            else if (memberName != "__type")
            {
                throw new JsonSerializationException(string.Format("Could not find member '{0}' on object of type '{1}'", memberName, targetType.GetType().Name));
            }
        }

        private void WriteMemberInfoProperty(JsonWriter writer, object value, MemberInfo member, string propertyName)
        {
            if (ReflectionUtils.IsIndexedProperty(member))
            {
                return;
            }
            object memberValue = ReflectionUtils.GetMemberValue(member, value);
            if (writer.SerializeStack.IndexOf(memberValue) != -1)
            {
                switch (this._referenceLoopHandling)
                {
                    case FluorineFx.Json.ReferenceLoopHandling.Error:
                        throw new JsonSerializationException("Self referencing loop");

                    case FluorineFx.Json.ReferenceLoopHandling.Ignore:
                        return;

                    case FluorineFx.Json.ReferenceLoopHandling.Serialize:
                        goto Label_0071;
                }
                throw new InvalidOperationException(string.Format("Unexpected ReferenceLoopHandling value: '{0}'", this._referenceLoopHandling));
            }
        Label_0071:
            writer.WritePropertyName((propertyName != null) ? propertyName : member.Name);
            this.SerializeValue(writer, memberValue);
        }

        public JsonConverterCollection Converters
        {
            get
            {
                if (this._converters == null)
                {
                    this._converters = new JsonConverterCollection();
                }
                return this._converters;
            }
        }

        public FluorineFx.Json.ReferenceLoopHandling ReferenceLoopHandling
        {
            get
            {
                return this._referenceLoopHandling;
            }
            set
            {
                if ((value < FluorineFx.Json.ReferenceLoopHandling.Error) || (value > FluorineFx.Json.ReferenceLoopHandling.Serialize))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._referenceLoopHandling = value;
            }
        }
    }
}

