namespace FluorineFx.Json
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;

    public class JsonReader : IDisposable
    {
        private StringBuffer _buffer;
        private char _currentChar;
        private State _currentState;
        private char _quoteChar;
        private TextReader _reader;
        private ArrayList _stack;
        private JsonToken _token;
        private int _top;
        private object _value;
        private Type _valueType;

        public JsonReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this._reader = reader;
            this._buffer = new StringBuffer(0x1000);
            this._currentState = State.Start;
            this._stack = new ArrayList();
            this._top = 0;
            this.Push(JsonType.None);
        }

        private void ClearCurrentChar()
        {
            this._currentChar = '\0';
        }

        public void Close()
        {
            this._currentState = State.Closed;
            this._token = JsonToken.None;
            this._value = null;
            this._valueType = null;
            if (this._reader != null)
            {
                this._reader.Close();
            }
            if (this._buffer != null)
            {
                this._buffer.Clear();
            }
        }

        private bool CurrentIsSeperator()
        {
            char ch = this._currentChar;
            if (ch <= ',')
            {
                switch (ch)
                {
                    case ')':
                        if (this._currentState != State.Constructor)
                        {
                            goto Label_0075;
                        }
                        return true;

                    case ',':
                        goto Label_002A;
                }
                goto Label_005D;
            }
            if (ch == '/')
            {
                return (this.HasNext() && (this.PeekNext() == '*'));
            }
            if ((ch != ']') && (ch != '}'))
            {
                goto Label_005D;
            }
        Label_002A:
            return true;
        Label_005D:
            if (char.IsWhiteSpace(this._currentChar))
            {
                return true;
            }
        Label_0075:
            return false;
        }

        private void Dispose(bool disposing)
        {
            if ((this._currentState != State.Closed) && disposing)
            {
                this.Close();
            }
        }

        private bool EatWhitespace(bool oneOrMore)
        {
            bool flag = false;
            while (char.IsWhiteSpace(this._currentChar))
            {
                flag = true;
                this.MoveNext();
            }
            return (!oneOrMore || flag);
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
            throw new JsonReaderException("Not a valid close JsonToken: " + token);
        }

        private bool HasNext()
        {
            return (this._reader.Peek() != -1);
        }

        private bool MatchValue(string value)
        {
            int num = 0;
            do
            {
                if (this._currentChar != value[num])
                {
                    break;
                }
                num++;
            }
            while ((num < value.Length) && this.MoveNext());
            return (num == value.Length);
        }

        private bool MatchValue(string value, bool noTrailingNonSeperatorCharacters)
        {
            bool flag = this.MatchValue(value);
            if (!noTrailingNonSeperatorCharacters)
            {
                return flag;
            }
            return (flag && (!this.MoveNext() || this.CurrentIsSeperator()));
        }

        private bool MoveNext()
        {
            int num = this._reader.Read();
            if (num != -1)
            {
                this._currentChar = (char) num;
                return true;
            }
            return false;
        }

        private bool MoveTo(char value)
        {
            while (this.MoveNext())
            {
                if (this._currentChar == value)
                {
                    return true;
                }
            }
            return false;
        }

        private void ParseComment()
        {
            this.MoveNext();
            if (this._currentChar != '*')
            {
                throw new JsonReaderException("Error parsing comment. Expected: *");
            }
            while (this.MoveNext())
            {
                if (this._currentChar == '*')
                {
                    if (this.MoveNext())
                    {
                        if (this._currentChar == '/')
                        {
                            break;
                        }
                        this._buffer.Append('*');
                        this._buffer.Append(this._currentChar);
                    }
                }
                else
                {
                    this._buffer.Append(this._currentChar);
                }
            }
            this.SetToken(JsonToken.Comment, this._buffer.ToString());
            this._buffer.Position = 0;
            this.ClearCurrentChar();
        }

        private void ParseConstructor()
        {
            if (this.MatchValue("new", true) && this.EatWhitespace(true))
            {
                while (char.IsLetter(this._currentChar))
                {
                    this._buffer.Append(this._currentChar);
                    this.MoveNext();
                }
                string strA = this._buffer.ToString();
                this._buffer.Position = 0;
                ArrayList list = new ArrayList();
                this.EatWhitespace(false);
                if ((this._currentChar == '(') && this.MoveNext())
                {
                    this._currentState = State.Constructor;
                    while (this.ParseValue())
                    {
                        list.Add(this._value);
                        this._currentState = State.Constructor;
                    }
                    if (string.CompareOrdinal(strA, "Date") == 0)
                    {
                        DateTime time = JavaScriptConvert.ConvertJavaScriptTicksToDateTime(Convert.ToInt64(list[0]));
                        this.SetToken(JsonToken.Date, time);
                    }
                    else
                    {
                        JavaScriptConstructor constructor = new JavaScriptConstructor(strA, new JavaScriptParameters(list));
                        if (this._currentState == State.ConstructorEnd)
                        {
                            this.SetToken(JsonToken.Constructor, constructor);
                        }
                    }
                    this.MoveNext();
                }
            }
        }

        private void ParseFalse()
        {
            if (!this.MatchValue(JavaScriptConvert.False, true))
            {
                throw new JsonReaderException("Error parsing boolean value.");
            }
            this.SetToken(JsonToken.Boolean, false);
        }

        private void ParseNull()
        {
            if (!this.MatchValue(JavaScriptConvert.Null, true))
            {
                throw new JsonReaderException("Error parsing null value.");
            }
            this.SetToken(JsonToken.Null);
        }

        private void ParseNumber()
        {
            object obj2;
            JsonToken integer;
            bool flag = false;
            do
            {
                if (this.CurrentIsSeperator())
                {
                    flag = true;
                }
                else
                {
                    this._buffer.Append(this._currentChar);
                }
            }
            while (!flag && this.MoveNext());
            if (this._buffer.ToString().IndexOf('.') == -1)
            {
                obj2 = Convert.ToInt64(this._buffer.ToString(), CultureInfo.InvariantCulture);
                integer = JsonToken.Integer;
            }
            else
            {
                obj2 = Convert.ToDouble(this._buffer.ToString(), CultureInfo.InvariantCulture);
                integer = JsonToken.Float;
            }
            this._buffer.Position = 0;
            this.SetToken(integer, obj2);
        }

        private bool ParseObject()
        {
            do
            {
                switch (this._currentChar)
                {
                    case ',':
                        this.SetToken(JsonToken.Undefined);
                        return true;

                    case '/':
                        this.ParseComment();
                        return true;

                    case '}':
                        this.SetToken(JsonToken.EndObject);
                        return true;
                }
                if (!char.IsWhiteSpace(this._currentChar))
                {
                    return this.ParseProperty();
                }
            }
            while (this.MoveNext());
            return false;
        }

        private bool ParsePostValue()
        {
            do
            {
                switch (this._currentChar)
                {
                    case ']':
                        this.SetToken(JsonToken.EndArray);
                        this.ClearCurrentChar();
                        return true;

                    case '}':
                        this.SetToken(JsonToken.EndObject);
                        this.ClearCurrentChar();
                        return true;

                    case ',':
                        this.SetStateBasedOnCurrent();
                        this.ClearCurrentChar();
                        return false;

                    case '/':
                        this.ParseComment();
                        return true;
                }
                if (!char.IsWhiteSpace(this._currentChar))
                {
                    throw new JsonReaderException("After parsing a value an unexpected character was encoutered: " + this._currentChar);
                }
                this.ClearCurrentChar();
            }
            while (this.MoveNext());
            return false;
        }

        private bool ParseProperty()
        {
            if (this.ValidIdentifierChar(this._currentChar))
            {
                this.ParseUnquotedProperty();
            }
            else
            {
                if ((this._currentChar != '"') && (this._currentChar != '\''))
                {
                    throw new JsonReaderException("Invalid property identifier character: " + this._currentChar);
                }
                this.ParseQuotedProperty(this._currentChar);
            }
            if (this._currentChar != ':')
            {
                this.MoveTo(':');
            }
            this.SetToken(JsonToken.PropertyName, this._buffer.ToString());
            this._buffer.Position = 0;
            return true;
        }

        private void ParseQuotedProperty(char quoteChar)
        {
            while (this.MoveNext())
            {
                if (this._currentChar == quoteChar)
                {
                    return;
                }
                this._buffer.Append(this._currentChar);
            }
            throw new JsonReaderException("Unclosed quoted property. Expected: " + quoteChar);
        }

        private void ParseString(char quote)
        {
            bool flag = false;
            while (!flag && this.MoveNext())
            {
                char ch = this._currentChar;
                switch (ch)
                {
                    case '"':
                    case '\'':
                    {
                        if (this._currentChar != quote)
                        {
                            goto Label_011A;
                        }
                        flag = true;
                        continue;
                    }
                    default:
                        if (ch != '\\')
                        {
                            goto Label_011A;
                        }
                        if (!this.MoveNext())
                        {
                            throw new JsonReaderException("Unterminated string. Expected delimiter: " + quote);
                        }
                        switch (this._currentChar)
                        {
                            case 'r':
                            {
                                this._buffer.Append('\r');
                                continue;
                            }
                            case 't':
                            {
                                this._buffer.Append('\t');
                                continue;
                            }
                            case 'u':
                            case 'x':
                            {
                                continue;
                            }
                            case 'n':
                            {
                                this._buffer.Append('\n');
                                continue;
                            }
                            case 'b':
                            {
                                this._buffer.Append('\b');
                                continue;
                            }
                            case 'f':
                            {
                                this._buffer.Append('\f');
                                continue;
                            }
                        }
                        break;
                }
                this._buffer.Append(this._currentChar);
                continue;
            Label_011A:
                this._buffer.Append(this._currentChar);
            }
            if (!flag)
            {
                throw new JsonReaderException("Unterminated string. Expected delimiter: " + quote);
            }
            this.ClearCurrentChar();
            this._currentState = State.PostValue;
            this._token = JsonToken.String;
            this._value = this._buffer.ToString();
            this._buffer.Position = 0;
            this._valueType = typeof(string);
            this._quoteChar = quote;
        }

        private void ParseTrue()
        {
            if (!this.MatchValue(JavaScriptConvert.True, true))
            {
                throw new JsonReaderException("Error parsing boolean value.");
            }
            this.SetToken(JsonToken.Boolean, true);
        }

        private void ParseUndefined()
        {
            if (!this.MatchValue(JavaScriptConvert.Undefined, true))
            {
                throw new JsonReaderException("Error parsing undefined value.");
            }
            this.SetToken(JsonToken.Undefined);
        }

        private void ParseUnquotedProperty()
        {
            this._buffer.Append(this._currentChar);
            while (this.MoveNext())
            {
                if (char.IsWhiteSpace(this._currentChar) || (this._currentChar == ':'))
                {
                    break;
                }
                if (!this.ValidIdentifierChar(this._currentChar))
                {
                    throw new JsonReaderException("Invalid JavaScript property identifier character: " + this._currentChar);
                }
                this._buffer.Append(this._currentChar);
            }
        }

        private bool ParseValue()
        {
            do
            {
                switch (this._currentChar)
                {
                    case ',':
                        this.SetToken(JsonToken.Undefined);
                        return true;

                    case '/':
                        this.ParseComment();
                        return true;

                    case '\'':
                    case '"':
                        this.ParseString(this._currentChar);
                        return true;

                    case ')':
                        if (this._currentState != State.Constructor)
                        {
                            throw new JsonReaderException("Unexpected character encountered while parsing value: " + this._currentChar);
                        }
                        this._currentState = State.ConstructorEnd;
                        return false;

                    case '[':
                        this.SetToken(JsonToken.StartArray);
                        return true;

                    case ']':
                        this.SetToken(JsonToken.EndArray);
                        return true;

                    case 'f':
                        this.ParseFalse();
                        return true;

                    case 't':
                        this.ParseTrue();
                        return true;

                    case 'u':
                        this.ParseUndefined();
                        return true;

                    case 'n':
                    {
                        if (!this.HasNext())
                        {
                            throw new JsonReaderException("Unexpected end");
                        }
                        char ch = this.PeekNext();
                        if (ch != 'u')
                        {
                            if (ch != 'e')
                            {
                                throw new JsonReaderException("Unexpected character encountered while parsing value: " + this._currentChar);
                            }
                            this.ParseConstructor();
                        }
                        else
                        {
                            this.ParseNull();
                        }
                        return true;
                    }
                    case '{':
                        this.SetToken(JsonToken.StartObject);
                        return true;

                    case '}':
                        this.SetToken(JsonToken.EndObject);
                        return true;
                }
                if (!char.IsWhiteSpace(this._currentChar))
                {
                    if ((!char.IsNumber(this._currentChar) && (this._currentChar != '-')) && (this._currentChar != '.'))
                    {
                        throw new JsonReaderException("Unexpected character encountered while parsing value: " + this._currentChar);
                    }
                    this.ParseNumber();
                    return true;
                }
            }
            while (this.MoveNext());
            return false;
        }

        private JsonType Peek()
        {
            return (JsonType) this._stack[this._top - 1];
        }

        private char PeekNext()
        {
            return (char) this._reader.Peek();
        }

        private JsonType Pop()
        {
            JsonType type = this.Peek();
            this._stack.RemoveAt(this._stack.Count - 1);
            this._top--;
            return type;
        }

        private void Push(JsonType value)
        {
            this._stack.Add(value);
            this._top++;
        }

        public bool Read()
        {
            bool flag2;
        Label_00B0:
            flag2 = true;
            if ((this._currentChar == '\0') && !this.MoveNext())
            {
                return false;
            }
            switch (this._currentState)
            {
                case State.Start:
                case State.Property:
                case State.ArrayStart:
                case State.Array:
                    return this.ParseValue();

                case State.Complete:
                case State.Closed:
                case State.Error:
                    goto Label_00B0;

                case State.ObjectStart:
                case State.Object:
                    return this.ParseObject();

                case State.PostValue:
                    if (!this.ParsePostValue())
                    {
                        goto Label_00B0;
                    }
                    return true;
            }
            throw new JsonReaderException("Unexpected state: " + this._currentState);
        }

        private void SetStateBasedOnCurrent()
        {
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
                    this._currentState = State.Finished;
                    break;

                default:
                    throw new JsonReaderException("While setting the reader state back to current object an unexpected JsonType was encountered: " + type);
            }
        }

        private void SetToken(JsonToken newToken)
        {
            this.SetToken(newToken, null);
        }

        private void SetToken(JsonToken newToken, object value)
        {
            this._token = newToken;
            switch (newToken)
            {
                case JsonToken.StartObject:
                    this._currentState = State.ObjectStart;
                    this.Push(JsonType.Object);
                    this.ClearCurrentChar();
                    break;

                case JsonToken.StartArray:
                    this._currentState = State.ArrayStart;
                    this.Push(JsonType.Array);
                    this.ClearCurrentChar();
                    break;

                case JsonToken.PropertyName:
                    this._currentState = State.Property;
                    this.ClearCurrentChar();
                    break;

                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Constructor:
                case JsonToken.Date:
                    this._currentState = State.PostValue;
                    break;

                case JsonToken.EndObject:
                    this.ValidateEnd(JsonToken.EndObject);
                    this.ClearCurrentChar();
                    this._currentState = State.PostValue;
                    break;

                case JsonToken.EndArray:
                    this.ValidateEnd(JsonToken.EndArray);
                    this.ClearCurrentChar();
                    this._currentState = State.PostValue;
                    break;
            }
            if (value != null)
            {
                this._value = value;
                this._valueType = value.GetType();
            }
            else
            {
                this._value = null;
                this._valueType = null;
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        private void ValidateEnd(JsonToken endToken)
        {
            JsonType type = this.Pop();
            if (this.GetTypeForCloseToken(endToken) != type)
            {
                throw new JsonReaderException(string.Format("JsonToken {0} is not valid for closing JsonType {1}.", endToken, type));
            }
        }

        private bool ValidIdentifierChar(char value)
        {
            return ((char.IsLetterOrDigit(this._currentChar) || (this._currentChar == '_')) || (this._currentChar == '$'));
        }

        public char QuoteChar
        {
            get
            {
                return this._quoteChar;
            }
        }

        public JsonToken TokenType
        {
            get
            {
                return this._token;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
        }

        public Type ValueType
        {
            get
            {
                return this._valueType;
            }
        }

        private enum State
        {
            Start,
            Complete,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            Closed,
            PostValue,
            Constructor,
            ConstructorEnd,
            Error,
            Finished
        }
    }
}

