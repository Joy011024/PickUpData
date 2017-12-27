namespace FluorineFx.Expression
{
    using antlr;
    using antlr.collections.impl;
    using System;
    using System.Collections;
    using System.IO;

    internal class ExpressionLexer : CharScanner, TokenStream
    {
        public const int AND = 8;
        public const int APOS = 0x31;
        public const int AT = 0x27;
        public const int BETWEEN = 13;
        public const int COLON = 0x2f;
        public const int COMMA = 0x1a;
        public const int DECIMAL_DIGIT = 0x33;
        public const int DIV = 0x13;
        public const int DOLLAR = 0x29;
        public const int DOT_ESCAPED = 0x30;
        public const int EOF = 1;
        public const int EQUAL = 0x20;
        public const int EXPONENT_PART = 0x36;
        public const int EXPR = 4;
        public const int FALSE = 6;
        public const int GREATER_THAN = 0x24;
        public const int GREATER_THAN_OR_EQUAL = 0x25;
        public const int HEX_DIGIT = 0x35;
        public const int HEXADECIMAL_INTEGER_LITERAL = 0x1c;
        public const int ID = 0x19;
        public const int IN = 11;
        public const int INTEGER_LITERAL = 0x1b;
        public const int INTEGER_TYPE_SUFFIX = 0x34;
        public const int IS = 12;
        public const int LBRACKET = 0x2a;
        public const int LCURLY = 0x2c;
        public const int LESS_THAN = 0x22;
        public const int LESS_THAN_OR_EQUAL = 0x23;
        public const int LIKE = 14;
        public const int LITERAL_date = 0x1f;
        public const int LPAREN = 0x16;
        public const int MINUS = 0x11;
        public const int MOD = 20;
        public const int NOT = 10;
        public const int NOT_EQUAL = 0x21;
        public const int NULL_LITERAL = 15;
        public const int NULL_TREE_LOOKAHEAD = 3;
        public const int NUMERIC_LITERAL = 50;
        public const int OPERAND = 5;
        public const int OR = 9;
        public const int PLUS = 0x10;
        public const int POUND = 0x18;
        public const int POWER = 0x15;
        public const int QMARK = 40;
        public const int RBRACKET = 0x2b;
        public const int RCURLY = 0x2d;
        public const int REAL_LITERAL = 0x1d;
        public const int REAL_TYPE_SUFFIX = 0x38;
        public const int RPAREN = 0x17;
        public const int SEMI = 0x2e;
        public const int SIGN = 0x37;
        public const int STAR = 0x12;
        public const int STRING_LITERAL = 30;
        public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
        public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
        public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
        public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());
        public static readonly BitSet tokenSet_4_ = new BitSet(mk_tokenSet_4_());
        public static readonly BitSet tokenSet_5_ = new BitSet(mk_tokenSet_5_());
        public static readonly BitSet tokenSet_6_ = new BitSet(mk_tokenSet_6_());
        public const int TRUE = 7;
        public const int WS = 0x26;

        public ExpressionLexer(InputBuffer ib) : this(new LexerSharedInputState(ib))
        {
        }

        public ExpressionLexer(LexerSharedInputState state) : base(state)
        {
            this.initialize();
        }

        public ExpressionLexer(Stream ins) : this((InputBuffer) new ByteBuffer(ins))
        {
        }

        public ExpressionLexer(TextReader r) : this((InputBuffer) new CharBuffer(r))
        {
        }

        private void initialize()
        {
            base.caseSensitiveLiterals = true;
            this.setCaseSensitive(true);
            base.literals = new Hashtable(100, 0.4f, null, Comparer.Default);
            base.literals.Add("IS", 12);
            base.literals.Add("NULL", 15);
            base.literals.Add("LIKE", 14);
            base.literals.Add("date", 0x1f);
            base.literals.Add("TRUE", 7);
            base.literals.Add("IN", 11);
            base.literals.Add("OR", 9);
            base.literals.Add("NOT", 10);
            base.literals.Add("AND", 8);
            base.literals.Add("FALSE", 6);
            base.literals.Add("BETWEEN", 13);
        }

        protected void mAPOS(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x31;
            int num3 = 0;
            num3 = base.text.Length;
            this.match('\'');
            base.text.Length = num3;
            this.match('\'');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mAT(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x27;
            this.match('@');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mCOLON(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x2f;
            this.match(':');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mCOMMA(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x1a;
            this.match(',');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        protected void mDECIMAL_DIGIT(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x33;
            this.matchRange('0', '9');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mDIV(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x13;
            this.match('/');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mDOLLAR(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x29;
            this.match('$');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mDOT_ESCAPED(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x30;
            this.match(@"\.");
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mEQUAL(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x20;
            this.match("=");
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        protected void mEXPONENT_PART(bool _createToken)
        {
            IToken token = null;
            int num3;
            int num4;
            bool flag;
            int length = base.text.Length;
            int num = 0x36;
            char ch = base.cached_LA1;
            if (ch == 'E')
            {
                this.match("E");
                while (true)
                {
                    flag = true;
                    if ((base.cached_LA1 == '+') || (base.cached_LA1 == '-'))
                    {
                        this.mSIGN(false);
                    }
                    else
                    {
                        num4 = 0;
                        goto Label_018D;
                    }
                }
            }
            if (ch != 'e')
            {
                throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
            }
            this.match("e");
            while (true)
            {
                flag = true;
                if ((base.cached_LA1 == '+') || (base.cached_LA1 == '-'))
                {
                    this.mSIGN(false);
                }
                else
                {
                    num3 = 0;
                    break;
                }
            }
            while (true)
            {
                flag = true;
                if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                {
                    this.mDECIMAL_DIGIT(false);
                }
                else
                {
                    if (num3 < 1)
                    {
                        throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                    }
                    goto Label_01B4;
                }
                num3++;
            }
        Label_018D:
            flag = true;
            if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
            {
                this.mDECIMAL_DIGIT(false);
            }
            else
            {
                if (num4 < 1)
                {
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                }
                goto Label_01B4;
            }
            num4++;
            goto Label_018D;
        Label_01B4:
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mGREATER_THAN(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x24;
            this.match('>');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mGREATER_THAN_OR_EQUAL(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x25;
            this.match(">=");
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        protected void mHEX_DIGIT(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x35;
            switch (base.cached_LA1)
            {
                case '0':
                    this.match('0');
                    break;

                case '1':
                    this.match('1');
                    break;

                case '2':
                    this.match('2');
                    break;

                case '3':
                    this.match('3');
                    break;

                case '4':
                    this.match('4');
                    break;

                case '5':
                    this.match('5');
                    break;

                case '6':
                    this.match('6');
                    break;

                case '7':
                    this.match('7');
                    break;

                case '8':
                    this.match('8');
                    break;

                case '9':
                    this.match('9');
                    break;

                case 'A':
                    this.match('A');
                    break;

                case 'B':
                    this.match('B');
                    break;

                case 'C':
                    this.match('C');
                    break;

                case 'D':
                    this.match('D');
                    break;

                case 'E':
                    this.match('E');
                    break;

                case 'F':
                    this.match('F');
                    break;

                case 'a':
                    this.match('a');
                    break;

                case 'b':
                    this.match('b');
                    break;

                case 'c':
                    this.match('c');
                    break;

                case 'd':
                    this.match('d');
                    break;

                case 'e':
                    this.match('e');
                    break;

                case 'f':
                    this.match('f');
                    break;

                default:
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
            }
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mHEXADECIMAL_INTEGER_LITERAL(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x1c;
            this.match("0x");
            int num3 = 0;
            while (true)
            {
                if (tokenSet_6_.member(base.cached_LA1))
                {
                    this.mHEX_DIGIT(false);
                }
                else
                {
                    if (num3 < 1)
                    {
                        throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                    }
                    if (tokenSet_5_.member(base.cached_LA1))
                    {
                        this.mINTEGER_TYPE_SUFFIX(false);
                    }
                    if ((_createToken && (token == null)) && (num != Token.SKIP))
                    {
                        token = this.makeToken(num);
                        token.setText(base.text.ToString(length, base.text.Length - length));
                    }
                    base.returnToken_ = token;
                    return;
                }
                num3++;
            }
        }

        public void mID(bool _createToken)
        {
            IToken token = null;
            bool flag;
            int length = base.text.Length;
            int num = 0x19;
            switch (base.cached_LA1)
            {
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    this.matchRange('A', 'Z');
                    break;

                case '_':
                    this.match('_');
                    break;

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    this.matchRange('a', 'z');
                    break;

                default:
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
            }
        Label_02DF:
            flag = true;
            switch (base.cached_LA1)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    this.matchRange('0', '9');
                    goto Label_02DF;

                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    this.matchRange('A', 'Z');
                    goto Label_02DF;

                case '\\':
                    this.mDOT_ESCAPED(false);
                    goto Label_02DF;

                case '_':
                    this.match('_');
                    goto Label_02DF;

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    this.matchRange('a', 'z');
                    goto Label_02DF;
            }
            num = this.testLiteralsTable(num);
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        protected void mINTEGER_TYPE_SUFFIX(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x34;
            if ((base.cached_LA1 == 'U') && (base.cached_LA2 == 'L'))
            {
                this.match("UL");
            }
            else if ((base.cached_LA1 == 'L') && (base.cached_LA2 == 'U'))
            {
                this.match("LU");
            }
            else if ((base.cached_LA1 == 'u') && (base.cached_LA2 == 'l'))
            {
                this.match("ul");
            }
            else if ((base.cached_LA1 == 'l') && (base.cached_LA2 == 'u'))
            {
                this.match("lu");
            }
            else if ((base.cached_LA1 == 'U') && (base.cached_LA2 == 'L'))
            {
                this.match("UL");
            }
            else if ((base.cached_LA1 == 'L') && (base.cached_LA2 == 'U'))
            {
                this.match("LU");
            }
            else if ((base.cached_LA1 == 'u') && (base.cached_LA2 == 'L'))
            {
                this.match("uL");
            }
            else if ((base.cached_LA1 == 'l') && (base.cached_LA2 == 'U'))
            {
                this.match("lU");
            }
            else if (base.cached_LA1 == 'U')
            {
                this.match("U");
            }
            else if (base.cached_LA1 == 'L')
            {
                this.match("L");
            }
            else if (base.cached_LA1 == 'u')
            {
                this.match("u");
            }
            else
            {
                if (base.cached_LA1 != 'l')
                {
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                }
                this.match("l");
            }
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        private static long[] mk_tokenSet_0_()
        {
            long[] numArray = new long[0x401];
            numArray[0] = 0x3ff400000000000L;
            for (int i = 1; i <= 0x400; i++)
            {
                numArray[i] = 0L;
            }
            return numArray;
        }

        private static long[] mk_tokenSet_1_()
        {
            int num;
            long[] numArray = new long[0x800];
            numArray[0] = -549755813889L;
            for (num = 1; num <= 0x3fe; num++)
            {
                numArray[num] = -1L;
            }
            numArray[0x3ff] = 0x7fffffffffffffffL;
            for (num = 0x400; num <= 0x7ff; num++)
            {
                numArray[num] = 0L;
            }
            return numArray;
        }

        private static long[] mk_tokenSet_2_()
        {
            long[] numArray = new long[0x401];
            numArray[0] = 0L;
            numArray[1] = 0x205000002050L;
            for (int i = 2; i <= 0x400; i++)
            {
                numArray[i] = 0L;
            }
            return numArray;
        }

        private static long[] mk_tokenSet_3_()
        {
            long[] numArray = new long[0x401];
            numArray[0] = 0x3ff000000000000L;
            numArray[1] = 0x2000000020L;
            for (int i = 2; i <= 0x400; i++)
            {
                numArray[i] = 0L;
            }
            return numArray;
        }

        private static long[] mk_tokenSet_4_()
        {
            long[] numArray = new long[0x401];
            numArray[0] = 0x3ff000000000000L;
            numArray[1] = 0x205000002050L;
            for (int i = 2; i <= 0x400; i++)
            {
                numArray[i] = 0L;
            }
            return numArray;
        }

        private static long[] mk_tokenSet_5_()
        {
            long[] numArray = new long[0x401];
            numArray[0] = 0L;
            numArray[1] = 0x20100000201000L;
            for (int i = 2; i <= 0x400; i++)
            {
                numArray[i] = 0L;
            }
            return numArray;
        }

        private static long[] mk_tokenSet_6_()
        {
            long[] numArray = new long[0x401];
            numArray[0] = 0x3ff000000000000L;
            numArray[1] = 0x7e0000007eL;
            for (int i = 2; i <= 0x400; i++)
            {
                numArray[i] = 0L;
            }
            return numArray;
        }

        public void mLBRACKET(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x2a;
            this.match('[');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mLCURLY(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x2c;
            this.match('{');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mLESS_THAN(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x22;
            this.match('<');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mLESS_THAN_OR_EQUAL(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x23;
            this.match("<=");
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mLPAREN(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x16;
            this.match('(');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mMINUS(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x11;
            this.match('-');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mMOD(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 20;
            this.match('%');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mNOT_EQUAL(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x21;
            this.match("<>");
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mNUMERIC_LITERAL(bool _createToken)
        {
            IToken token = null;
            int num6;
            int num12;
            bool flag5;
            int length = base.text.Length;
            int num = 50;
            bool flag = false;
            if (((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9')) && tokenSet_0_.member(base.cached_LA2))
            {
                int num3 = this.mark();
                flag = true;
                base.inputState.guessing++;
                try
                {
                    int num4 = 0;
                    goto Label_00CC;
                Label_006D:
                    if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                    {
                        this.mDECIMAL_DIGIT(false);
                    }
                    else
                    {
                        if (num4 < 1)
                        {
                            throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                        }
                        goto Label_00D1;
                    }
                    num4++;
                Label_00CC:
                    flag5 = true;
                    goto Label_006D;
                Label_00D1:
                    this.match('.');
                    this.mDECIMAL_DIGIT(false);
                }
                catch (RecognitionException)
                {
                    flag = false;
                }
                this.rewind(num3);
                base.inputState.guessing--;
            }
            if (!flag)
            {
                bool flag2 = false;
                if (((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9')) && tokenSet_3_.member(base.cached_LA2))
                {
                    int num7 = this.mark();
                    flag2 = true;
                    base.inputState.guessing++;
                    try
                    {
                        int num8 = 0;
                        goto Label_0331;
                    Label_02D2:
                        if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                        {
                            this.mDECIMAL_DIGIT(false);
                        }
                        else
                        {
                            if (num8 < 1)
                            {
                                throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                            }
                            goto Label_0336;
                        }
                        num8++;
                    Label_0331:
                        flag5 = true;
                        goto Label_02D2;
                    Label_0336:
                        this.mEXPONENT_PART(false);
                    }
                    catch (RecognitionException)
                    {
                        flag2 = false;
                    }
                    this.rewind(num7);
                    base.inputState.guessing--;
                }
                if (!flag2)
                {
                    bool flag3 = false;
                    if (((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9')) && tokenSet_4_.member(base.cached_LA2))
                    {
                        int num10 = this.mark();
                        flag3 = true;
                        base.inputState.guessing++;
                        try
                        {
                            int num11 = 0;
                            goto Label_04F7;
                        Label_0498:
                            if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                            {
                                this.mDECIMAL_DIGIT(false);
                            }
                            else
                            {
                                if (num11 < 1)
                                {
                                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                                }
                                goto Label_04FC;
                            }
                            num11++;
                        Label_04F7:
                            flag5 = true;
                            goto Label_0498;
                        Label_04FC:
                            this.mREAL_TYPE_SUFFIX(false);
                        }
                        catch (RecognitionException)
                        {
                            flag3 = false;
                        }
                        this.rewind(num10);
                        base.inputState.guessing--;
                    }
                    if (!flag3)
                    {
                        bool flag4 = false;
                        if (base.cached_LA1 == '.')
                        {
                            int num13 = this.mark();
                            flag4 = true;
                            base.inputState.guessing++;
                            try
                            {
                                this.match('.');
                                this.mDECIMAL_DIGIT(false);
                            }
                            catch (RecognitionException)
                            {
                                flag4 = false;
                            }
                            this.rewind(num13);
                            base.inputState.guessing--;
                        }
                        if (!flag4)
                        {
                            if ((base.cached_LA1 < '0') || (base.cached_LA1 > '9'))
                            {
                                throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                            }
                            int num15 = 0;
                            while (true)
                            {
                                flag5 = true;
                                if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                                {
                                    this.mDECIMAL_DIGIT(false);
                                }
                                else
                                {
                                    if (num15 < 1)
                                    {
                                        throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                                    }
                                    if (tokenSet_5_.member(base.cached_LA1))
                                    {
                                        this.mINTEGER_TYPE_SUFFIX(false);
                                    }
                                    if (0 == base.inputState.guessing)
                                    {
                                        num = 0x1b;
                                    }
                                    goto Label_083E;
                                }
                                num15++;
                            }
                        }
                        this.match('.');
                        int num14 = 0;
                        while (true)
                        {
                            flag5 = true;
                            if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                            {
                                this.mDECIMAL_DIGIT(false);
                            }
                            else
                            {
                                if (num14 < 1)
                                {
                                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                                }
                                if ((base.cached_LA1 == 'E') || (base.cached_LA1 == 'e'))
                                {
                                    this.mEXPONENT_PART(false);
                                }
                                if (tokenSet_2_.member(base.cached_LA1))
                                {
                                    this.mREAL_TYPE_SUFFIX(false);
                                }
                                if (0 == base.inputState.guessing)
                                {
                                    num = 0x1d;
                                }
                                goto Label_083E;
                            }
                            num14++;
                        }
                    }
                    num12 = 0;
                    goto Label_05A5;
                }
                int num9 = 0;
                while (true)
                {
                    flag5 = true;
                    if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                    {
                        this.mDECIMAL_DIGIT(false);
                    }
                    else
                    {
                        if (num9 < 1)
                        {
                            throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                        }
                        this.mEXPONENT_PART(false);
                        if (tokenSet_2_.member(base.cached_LA1))
                        {
                            this.mREAL_TYPE_SUFFIX(false);
                        }
                        if (0 == base.inputState.guessing)
                        {
                            num = 0x1d;
                        }
                        goto Label_083E;
                    }
                    num9++;
                }
            }
            int num5 = 0;
            while (true)
            {
                flag5 = true;
                if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                {
                    this.mDECIMAL_DIGIT(false);
                }
                else
                {
                    if (num5 < 1)
                    {
                        throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                    }
                    this.match('.');
                    num6 = 0;
                    break;
                }
                num5++;
            }
            while (true)
            {
                flag5 = true;
                if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
                {
                    this.mDECIMAL_DIGIT(false);
                }
                else
                {
                    if (num6 < 1)
                    {
                        throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                    }
                    if ((base.cached_LA1 == 'E') || (base.cached_LA1 == 'e'))
                    {
                        this.mEXPONENT_PART(false);
                    }
                    if (tokenSet_2_.member(base.cached_LA1))
                    {
                        this.mREAL_TYPE_SUFFIX(false);
                    }
                    if (0 == base.inputState.guessing)
                    {
                        num = 0x1d;
                    }
                    goto Label_083E;
                }
                num6++;
            }
        Label_05A5:
            flag5 = true;
            if ((base.cached_LA1 >= '0') && (base.cached_LA1 <= '9'))
            {
                this.mDECIMAL_DIGIT(false);
            }
            else
            {
                if (num12 < 1)
                {
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                }
                this.mREAL_TYPE_SUFFIX(false);
                if (0 == base.inputState.guessing)
                {
                    num = 0x1d;
                }
                goto Label_083E;
            }
            num12++;
            goto Label_05A5;
        Label_083E:
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mPLUS(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x10;
            this.match('+');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mPOUND(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x18;
            this.match('#');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mPOWER(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x15;
            this.match('^');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mQMARK(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 40;
            this.match('?');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mRBRACKET(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x2b;
            this.match(']');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mRCURLY(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x2d;
            this.match('}');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        protected void mREAL_TYPE_SUFFIX(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x38;
            switch (base.cached_LA1)
            {
                case 'd':
                    this.match('d');
                    break;

                case 'f':
                    this.match('f');
                    break;

                case 'm':
                    this.match('m');
                    break;

                case 'D':
                    this.match('D');
                    break;

                case 'F':
                    this.match('F');
                    break;

                case 'M':
                    this.match('M');
                    break;

                default:
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
            }
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mRPAREN(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x17;
            this.match(')');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mSEMI(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x2e;
            this.match(';');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        protected void mSIGN(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x37;
            switch (base.cached_LA1)
            {
                case '+':
                    this.match('+');
                    break;

                case '-':
                    this.match('-');
                    break;

                default:
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
            }
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mSTAR(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 0x12;
            this.match('*');
            if ((_createToken && (token == null)) && (num != Token.SKIP))
            {
                token = this.makeToken(num);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public void mSTRING_LITERAL(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int num = 30;
            int num3 = 0;
            num3 = base.text.Length;
            this.match('\'');
            base.text.Length = num3;
            while (true)
            {
                if ((base.cached_LA1 == '\'') && (base.cached_LA2 == '\''))
                {
                    this.mAPOS(false);
                }
                else if (tokenSet_1_.member(base.cached_LA1))
                {
                    this.matchNot('\'');
                }
                else
                {
                    num3 = base.text.Length;
                    this.match('\'');
                    base.text.Length = num3;
                    if ((_createToken && (token == null)) && (num != Token.SKIP))
                    {
                        token = this.makeToken(num);
                        token.setText(base.text.ToString(length, base.text.Length - length));
                    }
                    base.returnToken_ = token;
                    return;
                }
            }
        }

        public void mWS(bool _createToken)
        {
            IToken token = null;
            int length = base.text.Length;
            int sKIP = 0x26;
            switch (base.cached_LA1)
            {
                case '\t':
                    this.match('\t');
                    break;

                case '\n':
                    this.match('\n');
                    break;

                case '\r':
                    this.match('\r');
                    break;

                case ' ':
                    this.match(' ');
                    break;

                default:
                    throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
            }
            if (0 == base.inputState.guessing)
            {
                sKIP = Token.SKIP;
            }
            if ((_createToken && (token == null)) && (sKIP != Token.SKIP))
            {
                token = this.makeToken(sKIP);
                token.setText(base.text.ToString(length, base.text.Length - length));
            }
            base.returnToken_ = token;
        }

        public override IToken nextToken()
        {
            IToken token = null;
            while (true)
            {
                int num = 0;
                this.resetText();
                try
                {
                    try
                    {
                        switch (base.cached_LA1)
                        {
                            case '\t':
                            case '\n':
                            case '\r':
                            case ' ':
                                this.mWS(true);
                                token = base.returnToken_;
                                break;

                            case '#':
                                this.mPOUND(true);
                                token = base.returnToken_;
                                break;

                            case '$':
                                this.mDOLLAR(true);
                                token = base.returnToken_;
                                break;

                            case '%':
                                this.mMOD(true);
                                token = base.returnToken_;
                                break;

                            case '\'':
                                this.mSTRING_LITERAL(true);
                                token = base.returnToken_;
                                break;

                            case '(':
                                this.mLPAREN(true);
                                token = base.returnToken_;
                                break;

                            case ')':
                                this.mRPAREN(true);
                                token = base.returnToken_;
                                break;

                            case '*':
                                this.mSTAR(true);
                                token = base.returnToken_;
                                break;

                            case '+':
                                this.mPLUS(true);
                                token = base.returnToken_;
                                break;

                            case ',':
                                this.mCOMMA(true);
                                token = base.returnToken_;
                                break;

                            case '-':
                                this.mMINUS(true);
                                token = base.returnToken_;
                                break;

                            case '/':
                                this.mDIV(true);
                                token = base.returnToken_;
                                break;

                            case ':':
                                this.mCOLON(true);
                                token = base.returnToken_;
                                break;

                            case ';':
                                this.mSEMI(true);
                                token = base.returnToken_;
                                break;

                            case '=':
                                this.mEQUAL(true);
                                token = base.returnToken_;
                                break;

                            case '?':
                                this.mQMARK(true);
                                token = base.returnToken_;
                                break;

                            case '@':
                                this.mAT(true);
                                token = base.returnToken_;
                                break;

                            case 'A':
                            case 'B':
                            case 'C':
                            case 'D':
                            case 'E':
                            case 'F':
                            case 'G':
                            case 'H':
                            case 'I':
                            case 'J':
                            case 'K':
                            case 'L':
                            case 'M':
                            case 'N':
                            case 'O':
                            case 'P':
                            case 'Q':
                            case 'R':
                            case 'S':
                            case 'T':
                            case 'U':
                            case 'V':
                            case 'W':
                            case 'X':
                            case 'Y':
                            case 'Z':
                            case '_':
                            case 'a':
                            case 'b':
                            case 'c':
                            case 'd':
                            case 'e':
                            case 'f':
                            case 'g':
                            case 'h':
                            case 'i':
                            case 'j':
                            case 'k':
                            case 'l':
                            case 'm':
                            case 'n':
                            case 'o':
                            case 'p':
                            case 'q':
                            case 'r':
                            case 's':
                            case 't':
                            case 'u':
                            case 'v':
                            case 'w':
                            case 'x':
                            case 'y':
                            case 'z':
                                this.mID(true);
                                token = base.returnToken_;
                                break;

                            case '[':
                                this.mLBRACKET(true);
                                token = base.returnToken_;
                                break;

                            case '\\':
                                this.mDOT_ESCAPED(true);
                                token = base.returnToken_;
                                break;

                            case ']':
                                this.mRBRACKET(true);
                                token = base.returnToken_;
                                break;

                            case '^':
                                this.mPOWER(true);
                                token = base.returnToken_;
                                break;

                            case '{':
                                this.mLCURLY(true);
                                token = base.returnToken_;
                                break;

                            case '}':
                                this.mRCURLY(true);
                                token = base.returnToken_;
                                break;

                            default:
                                if ((base.cached_LA1 == '<') && (base.cached_LA2 == '>'))
                                {
                                    this.mNOT_EQUAL(true);
                                    token = base.returnToken_;
                                }
                                else if ((base.cached_LA1 == '<') && (base.cached_LA2 == '='))
                                {
                                    this.mLESS_THAN_OR_EQUAL(true);
                                    token = base.returnToken_;
                                }
                                else if ((base.cached_LA1 == '>') && (base.cached_LA2 == '='))
                                {
                                    this.mGREATER_THAN_OR_EQUAL(true);
                                    token = base.returnToken_;
                                }
                                else if ((base.cached_LA1 == '0') && (base.cached_LA2 == 'x'))
                                {
                                    this.mHEXADECIMAL_INTEGER_LITERAL(true);
                                    token = base.returnToken_;
                                }
                                else if (base.cached_LA1 == '<')
                                {
                                    this.mLESS_THAN(true);
                                    token = base.returnToken_;
                                }
                                else if (base.cached_LA1 == '>')
                                {
                                    this.mGREATER_THAN(true);
                                    token = base.returnToken_;
                                }
                                else if (tokenSet_0_.member(base.cached_LA1))
                                {
                                    this.mNUMERIC_LITERAL(true);
                                    token = base.returnToken_;
                                }
                                else
                                {
                                    if (base.cached_LA1 != CharScanner.EOF_CHAR)
                                    {
                                        throw new NoViableAltForCharException(base.cached_LA1, this.getFilename(), this.getLine(), this.getColumn());
                                    }
                                    this.uponEOF();
                                    base.returnToken_ = this.makeToken(1);
                                }
                                break;
                        }
                        if (null != base.returnToken_)
                        {
                            num = base.returnToken_.get_Type();
                            num = this.testLiteralsTable(num);
                            base.returnToken_.set_Type(num);
                            return base.returnToken_;
                        }
                    }
                    catch (RecognitionException exception)
                    {
                        throw new TokenStreamRecognitionException(exception);
                    }
                }
                catch (CharStreamException exception2)
                {
                    if (exception2 is CharStreamIOException)
                    {
                        throw new TokenStreamIOException(((CharStreamIOException) exception2).io);
                    }
                    throw new TokenStreamException(exception2.Message);
                }
            }
        }
    }
}

