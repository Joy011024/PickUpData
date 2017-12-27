namespace FluorineFx.Json
{
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.IO;

    public class JsonWriter : IDisposable
    {
        private State _currentState;
        private FluorineFx.Json.Formatting _formatting;
        private int _indentation;
        private char _indentChar;
        private char _quoteChar;
        private bool _quoteName;
        private ArrayList _serializeStack;
        private ArrayList _stack;
        private int _top;
        private TextWriter _writer;
        private static readonly State[,] stateArray = new State[,] { { State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error }, { State.ObjectStart, State.ObjectStart, State.Error, State.Error, State.ObjectStart, State.ObjectStart, State.Error, State.Error }, { State.ArrayStart, State.ArrayStart, State.Error, State.Error, State.ArrayStart, State.ArrayStart, State.Error, State.Error }, { State.Error, State.Error, State.Property, State.Property, State.Error, State.Error, State.Error, State.Error }, { State.Error, State.Property, State.ObjectStart, State.Object, State.ArrayStart, State.Array, State.Error, State.Error }, { State.Error, State.Object, State.Error, State.Error, State.Array, State.Array, State.Error, State.Error } };

        public JsonWriter(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException("textWriter");
            }
            this._writer = textWriter;
            this._quoteChar = '"';
            this._quoteName = true;
            this._indentChar = ' ';
            this._indentation = 2;
            this._formatting = FluorineFx.Json.Formatting.None;
            this._stack = new ArrayList(1);
            this._stack.Add(JsonType.None);
            this._currentState = State.Start;
        }

        private void AutoComplete(JsonToken tokenBeingWritten)
        {
            int num;
            switch (tokenBeingWritten)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Date:
                    num = 5;
                    break;

                default:
                    num = (int) tokenBeingWritten;
                    break;
            }
            State state = stateArray[num, (int) this._currentState];
            if (state == State.Error)
            {
                throw new JsonWriterException(string.Format("Token {0} in state {1} would result in an invalid JavaScript object.", tokenBeingWritten.ToString(), this._currentState.ToString()));
            }
            if (((this._currentState == State.Object) || (this._currentState == State.Array)) && (tokenBeingWritten != JsonToken.Comment))
            {
                this._writer.Write(',');
            }
            else if ((this._currentState == State.Property) && (this._formatting == FluorineFx.Json.Formatting.Indented))
            {
                this._writer.Write(' ');
            }
            if ((tokenBeingWritten == JsonToken.PropertyName) || (this.WriteState == FluorineFx.Json.WriteState.Array))
            {
                this.WriteIndent();
            }
            this._currentState = state;
        }

        private void AutoCompleteAll()
        {
            while (this._top > 0)
            {
                this.WriteEnd();
            }
        }

        private void AutoCompleteClose(JsonToken tokenBeingClosed)
        {
            int num2;
            int num = 0;
            for (num2 = 0; num2 < this._top; num2++)
            {
                int num3 = this._top - num2;
                if (((JsonType) this._stack[num3]) == this.GetTypeForCloseToken(tokenBeingClosed))
                {
                    num = num2 + 1;
                    break;
                }
            }
            if (num == 0)
            {
                throw new JsonWriterException("No token to close.");
            }
            for (num2 = 0; num2 < num; num2++)
            {
                JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
                if ((this._currentState != State.ObjectStart) && (this._currentState != State.ArrayStart))
                {
                    this.WriteIndent();
                }
                switch (closeTokenForType)
                {
                    case JsonToken.EndObject:
                        this._writer.Write("}");
                        break;

                    case JsonToken.EndArray:
                        this._writer.Write("]");
                        break;

                    default:
                        throw new JsonWriterException("Invalid JsonToken: " + closeTokenForType);
                }
            }
            JsonType type = this.Peek();
            switch (type)
            {
                case JsonType.Object:
                    this._currentState = State.Object;
                    break;

                case JsonType.Array:
                    this._currentState = State.Array;
                    break;

                case JsonType.None:
                    this._currentState = State.Start;
                    break;

                default:
                    throw new JsonWriterException("Unknown JsonType: " + type);
            }
        }

        public void Close()
        {
            this.AutoCompleteAll();
            this._writer.Close();
        }

        private void Dispose(bool disposing)
        {
            if (this.WriteState != FluorineFx.Json.WriteState.Closed)
            {
                this.Close();
            }
        }

        public void Flush()
        {
            this._writer.Flush();
        }

        private JsonToken GetCloseTokenForType(JsonType type)
        {
            switch (type)
            {
                case JsonType.Object:
                    return JsonToken.EndObject;

                case JsonType.Array:
                    return JsonToken.EndArray;
            }
            throw new JsonWriterException("No close token for type: " + type);
        }

        private JsonType GetTypeForCloseToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return JsonType.Object;

                case JsonToken.EndArray:
                    return JsonType.Array;
            }
            throw new JsonWriterException("No type for token: " + token);
        }

        private JsonType Peek()
        {
            return (JsonType) this._stack[this._top];
        }

        private JsonType Pop()
        {
            JsonType type = this.Peek();
            this._top--;
            return type;
        }

        private void Push(JsonType value)
        {
            this._top++;
            if (this._stack.Count <= this._top)
            {
                this._stack.Add(value);
            }
            else
            {
                this._stack[this._top] = value;
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        public void WriteComment(string text)
        {
            this.AutoComplete(JsonToken.Comment);
            this._writer.Write("/*");
            this._writer.Write(text);
            this._writer.Write("*/");
        }

        public void WriteEnd()
        {
            this.WriteEnd(this.Peek());
        }

        private void WriteEnd(JsonType type)
        {
            switch (type)
            {
                case JsonType.Object:
                    this.WriteEndObject();
                    break;

                case JsonType.Array:
                    this.WriteEndArray();
                    break;

                default:
                    throw new JsonWriterException("Unexpected type when writing end: " + type);
            }
        }

        public void WriteEndArray()
        {
            this.AutoCompleteClose(JsonToken.EndArray);
        }

        public void WriteEndObject()
        {
            this.AutoCompleteClose(JsonToken.EndObject);
        }

        private void WriteIndent()
        {
            if (this._formatting == FluorineFx.Json.Formatting.Indented)
            {
                this._writer.Write(Environment.NewLine);
                for (int i = 0; i < this._top; i++)
                {
                    for (int j = 0; j < this._indentation; j++)
                    {
                        this._writer.Write(this._indentChar);
                    }
                }
            }
        }

        public void WriteNull()
        {
            this.WriteValueInternal(JavaScriptConvert.Null, JsonToken.Null);
        }

        public void WritePropertyName(string name)
        {
            this.AutoComplete(JsonToken.PropertyName);
            if (this._quoteName)
            {
                this._writer.Write(this._quoteChar);
            }
            this._writer.Write(name);
            if (this._quoteName)
            {
                this._writer.Write(this._quoteChar);
            }
            this._writer.Write(':');
        }

        public void WriteRaw(string javaScript)
        {
            this.WriteValueInternal(javaScript, JsonToken.Undefined);
        }

        public void WriteStartArray()
        {
            this.AutoComplete(JsonToken.StartArray);
            this.Push(JsonType.Array);
            this._writer.Write("[");
        }

        public void WriteStartObject()
        {
            this.AutoComplete(JsonToken.StartObject);
            this.Push(JsonType.Object);
            this._writer.Write("{");
        }

        public void WriteStringArray(IEnumerable values)
        {
            if (values == null)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteStartArray();
                foreach (object obj2 in values)
                {
                    if (JavaScriptConvert.IsNull(obj2))
                    {
                        this.WriteNull();
                    }
                    else
                    {
                        this.WriteValue(obj2.ToString());
                    }
                }
                this.WriteEndArray();
            }
        }

        public void WriteUndefined()
        {
            this.WriteValueInternal(JavaScriptConvert.Undefined, JsonToken.Undefined);
        }

        public void WriteValue(bool value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Boolean);
        }

        public void WriteValue(byte value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        public void WriteValue(char value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        public void WriteValue(DateTime value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Date);
        }

        public void WriteValue(decimal value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Float);
        }

        public void WriteValue(double value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Float);
        }

        public void WriteValue(short value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        public void WriteValue(int value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        public void WriteValue(long value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public void WriteValue(sbyte value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        public void WriteValue(float value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Float);
        }

        public void WriteValue(string value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value, this._quoteChar), JsonToken.String);
        }

        [CLSCompliant(false)]
        public void WriteValue(ushort value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public void WriteValue(uint value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public void WriteValue(ulong value)
        {
            this.WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
        }

        private void WriteValueInternal(string value, JsonToken token)
        {
            this.AutoComplete(token);
            this._writer.Write(value);
        }

        public void WriteWhitespace(string ws)
        {
            if (ws != null)
            {
                if (!StringUtils.IsWhiteSpace(ws))
                {
                    throw new JsonWriterException("Only white space characters should be used.");
                }
                this._writer.Write(ws);
            }
        }

        public FluorineFx.Json.Formatting Formatting
        {
            get
            {
                return this._formatting;
            }
            set
            {
                this._formatting = value;
            }
        }

        public int Indentation
        {
            get
            {
                return this._indentation;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Indentation value must be greater than 0.");
                }
                this._indentation = value;
            }
        }

        public char IndentChar
        {
            get
            {
                return this._indentChar;
            }
            set
            {
                this._indentChar = value;
            }
        }

        public char QuoteChar
        {
            get
            {
                return this._quoteChar;
            }
            set
            {
                if ((value != '"') && (value != '\''))
                {
                    throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
                }
                this._quoteChar = value;
            }
        }

        public bool QuoteName
        {
            get
            {
                return this._quoteName;
            }
            set
            {
                this._quoteName = value;
            }
        }

        internal ArrayList SerializeStack
        {
            get
            {
                if (this._serializeStack == null)
                {
                    this._serializeStack = new ArrayList();
                }
                return this._serializeStack;
            }
        }

        public FluorineFx.Json.WriteState WriteState
        {
            get
            {
                switch (this._currentState)
                {
                    case State.Start:
                        return FluorineFx.Json.WriteState.Start;

                    case State.Property:
                        return FluorineFx.Json.WriteState.Property;

                    case State.ObjectStart:
                    case State.Object:
                        return FluorineFx.Json.WriteState.Object;

                    case State.ArrayStart:
                    case State.Array:
                        return FluorineFx.Json.WriteState.Array;

                    case State.Closed:
                        return FluorineFx.Json.WriteState.Closed;

                    case State.Error:
                        return FluorineFx.Json.WriteState.Error;
                }
                throw new JsonWriterException("Invalid state: " + this._currentState);
            }
        }

        private enum State
        {
            Start,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            Closed,
            Error
        }
    }
}

