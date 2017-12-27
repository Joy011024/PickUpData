namespace FluorineFx.Expression
{
    using antlr;
    using antlr.collections.impl;
    using System;

    internal class ExpressionParser : LLkParser
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
        public static readonly string[] tokenNames_ = new string[] { 
            "\"<0>\"", "\"EOF\"", "\"<2>\"", "\"NULL_TREE_LOOKAHEAD\"", "\"EXPR\"", "\"OPERAND\"", "\"FALSE\"", "\"TRUE\"", "\"AND\"", "\"OR\"", "\"NOT\"", "\"IN\"", "\"IS\"", "\"BETWEEN\"", "\"LIKE\"", "\"NULL\"", 
            "\"PLUS\"", "\"MINUS\"", "\"STAR\"", "\"DIV\"", "\"MOD\"", "\"POWER\"", "\"LPAREN\"", "\"RPAREN\"", "\"POUND\"", "\"ID\"", "\"COMMA\"", "\"INTEGER_LITERAL\"", "\"HEXADECIMAL_INTEGER_LITERAL\"", "\"REAL_LITERAL\"", "\"STRING_LITERAL\"", "\"date\"", 
            "\"EQUAL\"", "\"NOT_EQUAL\"", "\"LESS_THAN\"", "\"LESS_THAN_OR_EQUAL\"", "\"GREATER_THAN\"", "\"GREATER_THAN_OR_EQUAL\"", "\"WS\"", "\"AT\"", "\"QMARK\"", "\"DOLLAR\"", "\"LBRACKET\"", "\"RBRACKET\"", "\"LCURLY\"", "\"RCURLY\"", "\"SEMI\"", "\"COLON\"", 
            "\"DOT_ESCAPED\"", "\"APOS\"", "\"NUMERIC_LITERAL\"", "\"DECIMAL_DIGIT\"", "\"INTEGER_TYPE_SUFFIX\"", "\"HEX_DIGIT\"", "\"EXPONENT_PART\"", "\"SIGN\"", "\"REAL_TYPE_SUFFIX\""
         };
        public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
        public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
        public static readonly BitSet tokenSet_10_ = new BitSet(mk_tokenSet_10_());
        public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
        public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());
        public static readonly BitSet tokenSet_4_ = new BitSet(mk_tokenSet_4_());
        public static readonly BitSet tokenSet_5_ = new BitSet(mk_tokenSet_5_());
        public static readonly BitSet tokenSet_6_ = new BitSet(mk_tokenSet_6_());
        public static readonly BitSet tokenSet_7_ = new BitSet(mk_tokenSet_7_());
        public static readonly BitSet tokenSet_8_ = new BitSet(mk_tokenSet_8_());
        public static readonly BitSet tokenSet_9_ = new BitSet(mk_tokenSet_9_());
        public const int TRUE = 7;
        public const int WS = 0x26;

        public ExpressionParser(ParserSharedInputState state) : base(state, 2)
        {
            this.initialize();
        }

        public ExpressionParser(TokenBuffer tokenBuf) : this(tokenBuf, 2)
        {
        }

        public ExpressionParser(TokenStream lexer) : this(lexer, 2)
        {
        }

        protected ExpressionParser(TokenBuffer tokenBuf, int k) : base(tokenBuf, k)
        {
            this.initialize();
        }

        protected ExpressionParser(TokenStream lexer, int k) : base(lexer, k)
        {
            this.initialize();
        }

        public void argument()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                this.expression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_10_);
            }
            base.returnAST = root;
        }

        public void betweenExpr()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            FluorineAST returnAST = null;
            FluorineAST east3 = null;
            try
            {
                this.sumExpr();
                if (0 == base.inputState.guessing)
                {
                    returnAST = base.returnAST;
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                ListInitializerNode node = null;
                node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.ListInitializerNode");
                base.astFactory.makeASTRoot(ref pair, node);
                this.match(8);
                this.sumExpr();
                if (0 == base.inputState.guessing)
                {
                    east3 = base.returnAST;
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_3_);
            }
            base.returnAST = root;
        }

        public void boolLiteral()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                if (this.LA(1) != 7)
                {
                    if (this.LA(1) != 6)
                    {
                        throw new NoViableAltException(this.LT(1), this.getFilename());
                    }
                    BooleanLiteralNode node2 = null;
                    node2 = base.astFactory.create(this.LT(1), "FluorineFx.Expression.BooleanLiteralNode");
                    base.astFactory.addASTChild(ref pair, node2);
                    this.match(6);
                    root = pair.root;
                }
                else
                {
                    BooleanLiteralNode node = null;
                    node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.BooleanLiteralNode");
                    base.astFactory.addASTChild(ref pair, node);
                    this.match(7);
                    root = pair.root;
                }
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        public void dateLiteral()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                DateLiteralNode node = null;
                node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.DateLiteralNode");
                base.astFactory.makeASTRoot(ref pair, node);
                this.match(0x1f);
                this.match(0x16);
                FluorineAST east2 = null;
                east2 = base.astFactory.create(this.LT(1));
                base.astFactory.addASTChild(ref pair, east2);
                this.match(30);
                if (this.LA(1) == 0x1a)
                {
                    this.match(0x1a);
                    FluorineAST east3 = null;
                    east3 = base.astFactory.create(this.LT(1));
                    base.astFactory.addASTChild(ref pair, east3);
                    this.match(30);
                }
                else if (this.LA(1) != 0x17)
                {
                    throw new NoViableAltException(this.LT(1), this.getFilename());
                }
                this.match(0x17);
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        public void expr()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                this.expression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                this.match(1);
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_0_);
            }
            base.returnAST = root;
        }

        public void expression()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                this.logicalOrExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_1_);
            }
            base.returnAST = root;
        }

        public void function()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                this.match(0x18);
                FunctionNode node = null;
                node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.FunctionNode");
                base.astFactory.makeASTRoot(ref pair, node);
                this.match(0x19);
                this.methodArgs();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        public void functionOrVar()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                bool flag = false;
                if (this.LA(1) == 0x18)
                {
                    int num = this.mark();
                    flag = true;
                    base.inputState.guessing++;
                    try
                    {
                        this.match(0x18);
                        this.match(0x19);
                        this.match(0x16);
                    }
                    catch (RecognitionException)
                    {
                        flag = false;
                    }
                    this.rewind(num);
                    base.inputState.guessing--;
                }
                if (flag)
                {
                    this.function();
                    if (0 == base.inputState.guessing)
                    {
                        base.astFactory.addASTChild(ref pair, base.returnAST);
                    }
                    root = pair.root;
                }
                else
                {
                    if (this.LA(1) != 0x19)
                    {
                        throw new NoViableAltException(this.LT(1), this.getFilename());
                    }
                    this.var();
                    if (0 == base.inputState.guessing)
                    {
                        base.astFactory.addASTChild(ref pair, base.returnAST);
                    }
                    root = pair.root;
                }
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        public FluorineAST getAST()
        {
            return (FluorineAST) base.returnAST;
        }

        private string GetRelationalOperatorNodeType(string op)
        {
            switch (op.ToLower())
            {
                case "=":
                    return "FluorineFx.Expression.OpEqual";

                case "<>":
                    return "FluorineFx.Expression.OpNotEqual";

                case "<":
                    return "FluorineFx.Expression.OpLess";

                case "<=":
                    return "FluorineFx.Expression.OpLessOrEqual";

                case ">":
                    return "FluorineFx.Expression.OpGreater";

                case ">=":
                    return "FluorineFx.Expression.OpGreaterOrEqual";

                case "in":
                    return "FluorineFx.Expression.OpIn";

                case "is":
                    return "FluorineFx.Expression.OpIs";

                case "between":
                    return "FluorineFx.Expression.OpBetween";

                case "like":
                    return "FluorineFx.Expression.OpLike";
            }
            throw new ArgumentException("Node type for operator '" + op + "' is not defined.");
        }

        protected void initialize()
        {
            base.tokenNames = tokenNames_;
            this.initializeFactory();
        }

        public static void initializeASTFactory(ASTFactory factory)
        {
            factory.setMaxNodeType(0x38);
        }

        private void initializeFactory()
        {
            if (base.astFactory == null)
            {
                base.astFactory = new ASTFactory("FluorineFx.Expression.FluorineAST");
            }
            initializeASTFactory(base.astFactory);
        }

        public void listInitializer()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                bool flag;
                ListInitializerNode node = null;
                node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.ListInitializerNode");
                base.astFactory.makeASTRoot(ref pair, node);
                this.match(0x16);
                this.primaryExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                goto Label_00DB;
            Label_0081:
                if (this.LA(1) != 0x1a)
                {
                    goto Label_00E0;
                }
                this.match(0x1a);
                this.primaryExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
            Label_00DB:
                flag = true;
                goto Label_0081;
            Label_00E0:
                this.match(0x17);
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_3_);
            }
            base.returnAST = root;
        }

        public void literal()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                switch (this.LA(1))
                {
                    case 6:
                    case 7:
                        this.boolLiteral();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        root = pair.root;
                        goto Label_02AC;

                    case 15:
                    {
                        NullLiteralNode node = null;
                        node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.NullLiteralNode");
                        base.astFactory.addASTChild(ref pair, node);
                        this.match(15);
                        root = pair.root;
                        goto Label_02AC;
                    }
                    case 0x1b:
                    {
                        IntLiteralNode node2 = null;
                        node2 = base.astFactory.create(this.LT(1), "FluorineFx.Expression.IntLiteralNode");
                        base.astFactory.addASTChild(ref pair, node2);
                        this.match(0x1b);
                        root = pair.root;
                        goto Label_02AC;
                    }
                    case 0x1c:
                    {
                        HexLiteralNode node3 = null;
                        node3 = base.astFactory.create(this.LT(1), "FluorineFx.Expression.HexLiteralNode");
                        base.astFactory.addASTChild(ref pair, node3);
                        this.match(0x1c);
                        root = pair.root;
                        goto Label_02AC;
                    }
                    case 0x1d:
                    {
                        RealLiteralNode node4 = null;
                        node4 = base.astFactory.create(this.LT(1), "FluorineFx.Expression.RealLiteralNode");
                        base.astFactory.addASTChild(ref pair, node4);
                        this.match(0x1d);
                        root = pair.root;
                        goto Label_02AC;
                    }
                    case 30:
                    {
                        StringLiteralNode node5 = null;
                        node5 = base.astFactory.create(this.LT(1), "FluorineFx.Expression.StringLiteralNode");
                        base.astFactory.addASTChild(ref pair, node5);
                        this.match(30);
                        root = pair.root;
                        goto Label_02AC;
                    }
                    case 0x1f:
                        this.dateLiteral();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        root = pair.root;
                        goto Label_02AC;
                }
                throw new NoViableAltException(this.LT(1), this.getFilename());
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
        Label_02AC:
            base.returnAST = root;
        }

        public void logicalAndExpression()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST east = null;
            try
            {
                bool flag;
                this.relationalExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                goto Label_00D3;
            Label_004D:
                if (this.LA(1) != 8)
                {
                    goto Label_00DB;
                }
                OpAND pand = null;
                pand = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpAND");
                base.astFactory.makeASTRoot(ref pair, pand);
                this.match(8);
                this.relationalExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
            Label_00D3:
                flag = true;
                goto Label_004D;
            Label_00DB:
                east = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_2_);
            }
            base.returnAST = east;
        }

        public void logicalOrExpression()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST east = null;
            try
            {
                bool flag;
                this.logicalAndExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                goto Label_00D5;
            Label_004D:
                if (this.LA(1) != 9)
                {
                    goto Label_00DD;
                }
                OpOR por = null;
                por = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpOR");
                base.astFactory.makeASTRoot(ref pair, por);
                this.match(9);
                this.logicalAndExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
            Label_00D5:
                flag = true;
                goto Label_004D;
            Label_00DD:
                east = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_1_);
            }
            base.returnAST = east;
        }

        public void methodArgs()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                bool flag;
                this.match(0x16);
                if (!tokenSet_5_.member(this.LA(1)))
                {
                    goto Label_00CC;
                }
                this.argument();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                goto Label_00C4;
            Label_006E:
                if (this.LA(1) != 0x1a)
                {
                    goto Label_00F6;
                }
                this.match(0x1a);
                this.argument();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
            Label_00C4:
                flag = true;
                goto Label_006E;
            Label_00CC:
                if (this.LA(1) != 0x17)
                {
                    throw new NoViableAltException(this.LT(1), this.getFilename());
                }
            Label_00F6:
                this.match(0x17);
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        private static long[] mk_tokenSet_0_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 2L;
            return numArray3;
        }

        private static long[] mk_tokenSet_1_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x4800002L;
            return numArray3;
        }

        private static long[] mk_tokenSet_10_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x4800000L;
            return numArray3;
        }

        private static long[] mk_tokenSet_2_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x4800202L;
            return numArray3;
        }

        private static long[] mk_tokenSet_3_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x4800302L;
            return numArray3;
        }

        private static long[] mk_tokenSet_4_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x3f04807f02L;
            return numArray3;
        }

        private static long[] mk_tokenSet_5_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0xfb4384c0L;
            return numArray3;
        }

        private static long[] mk_tokenSet_6_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x3f04837f02L;
            return numArray3;
        }

        private static long[] mk_tokenSet_7_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x3f049f7f02L;
            return numArray3;
        }

        private static long[] mk_tokenSet_8_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0xfb4080c0L;
            return numArray3;
        }

        private static long[] mk_tokenSet_9_()
        {
            long[] numArray3 = new long[2];
            numArray3[0] = 0x3f04bf7f02L;
            return numArray3;
        }

        public void parenExpr()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                this.match(0x16);
                this.expression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                this.match(0x17);
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        public void pattern()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                switch (this.LA(1))
                {
                    case 1:
                    case 8:
                    case 9:
                    case 0x17:
                    case 0x1a:
                        root = pair.root;
                        goto Label_0183;

                    case 6:
                    case 7:
                    case 15:
                    case 0x1b:
                    case 0x1c:
                    case 0x1d:
                    case 30:
                    case 0x1f:
                        this.literal();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        root = pair.root;
                        goto Label_0183;

                    case 0x18:
                    case 0x19:
                        this.functionOrVar();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        root = pair.root;
                        goto Label_0183;
                }
                throw new NoViableAltException(this.LT(1), this.getFilename());
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_3_);
            }
        Label_0183:
            base.returnAST = root;
        }

        public void powExpr()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                this.unaryExpression();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                if (this.LA(1) == 0x15)
                {
                    OpPOWER ppower = null;
                    ppower = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpPOWER");
                    base.astFactory.makeASTRoot(ref pair, ppower);
                    this.match(0x15);
                    this.unaryExpression();
                    if (0 == base.inputState.guessing)
                    {
                        base.astFactory.addASTChild(ref pair, base.returnAST);
                    }
                }
                else if (!tokenSet_7_.member(this.LA(1)))
                {
                    throw new NoViableAltException(this.LT(1), this.getFilename());
                }
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_7_);
            }
            base.returnAST = root;
        }

        public void primaryExpression()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                switch (this.LA(1))
                {
                    case 6:
                    case 7:
                    case 15:
                    case 0x1b:
                    case 0x1c:
                    case 0x1d:
                    case 30:
                    case 0x1f:
                        this.literal();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        root = pair.root;
                        goto Label_0242;

                    case 0x16:
                        this.parenExpr();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        root = pair.root;
                        goto Label_0242;

                    case 0x18:
                    case 0x19:
                        this.functionOrVar();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        if (0 == base.inputState.guessing)
                        {
                            root = pair.root;
                            root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "expression", "FluorineFx.Expression.Expression"), root });
                            pair.root = root;
                            if ((root != null) && (null != root.getFirstChild()))
                            {
                                pair.child = root.getFirstChild();
                            }
                            else
                            {
                                pair.child = root;
                            }
                            pair.advanceChildToEnd();
                        }
                        root = pair.root;
                        goto Label_0242;
                }
                throw new NoViableAltException(this.LT(1), this.getFilename());
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
        Label_0242:
            base.returnAST = root;
        }

        public void prodExpr()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST east = null;
            try
            {
                bool flag;
                this.powExpr();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                goto Label_019A;
            Label_004D:
                if ((this.LA(1) < 0x12) || (this.LA(1) > 20))
                {
                    goto Label_01A2;
                }
                switch (this.LA(1))
                {
                    case 0x12:
                    {
                        OpMULTIPLY pmultiply = null;
                        pmultiply = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpMULTIPLY");
                        base.astFactory.makeASTRoot(ref pair, pmultiply);
                        this.match(0x12);
                        break;
                    }
                    case 0x13:
                    {
                        OpDIVIDE pdivide = null;
                        pdivide = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpDIVIDE");
                        base.astFactory.makeASTRoot(ref pair, pdivide);
                        this.match(0x13);
                        break;
                    }
                    case 20:
                    {
                        OpMODULUS pmodulus = null;
                        pmodulus = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpMODULUS");
                        base.astFactory.makeASTRoot(ref pair, pmodulus);
                        this.match(20);
                        break;
                    }
                    default:
                        throw new NoViableAltException(this.LT(1), this.getFilename());
                }
                this.powExpr();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
            Label_019A:
                flag = true;
                goto Label_004D;
            Label_01A2:
                east = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_6_);
            }
            base.returnAST = east;
        }

        public void relationalExpression()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            FluorineAST returnAST = null;
            FluorineAST east3 = null;
            try
            {
                this.sumExpr();
                if (0 == base.inputState.guessing)
                {
                    returnAST = base.returnAST;
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                switch (this.LA(1))
                {
                    case 8:
                    case 9:
                    case 1:
                    case 0x17:
                    case 0x1a:
                        break;

                    case 11:
                        this.match(11);
                        this.listInitializer();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        if (0 == base.inputState.guessing)
                        {
                            root = pair.root;
                            root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "IN", this.GetRelationalOperatorNodeType("IN")), root });
                            pair.root = root;
                            if ((root != null) && (null != root.getFirstChild()))
                            {
                                pair.child = root.getFirstChild();
                            }
                            else
                            {
                                pair.child = root;
                            }
                            pair.advanceChildToEnd();
                        }
                        break;

                    case 12:
                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                    case 0x24:
                    case 0x25:
                        this.relationalOperator();
                        if (0 == base.inputState.guessing)
                        {
                            east3 = base.returnAST;
                        }
                        this.sumExpr();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        if (0 == base.inputState.guessing)
                        {
                            root = pair.root;
                            root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, east3.getText(), this.GetRelationalOperatorNodeType(east3.getText())), root });
                            pair.root = root;
                            if ((root != null) && (null != root.getFirstChild()))
                            {
                                pair.child = root.getFirstChild();
                            }
                            else
                            {
                                pair.child = root;
                            }
                            pair.advanceChildToEnd();
                        }
                        break;

                    case 13:
                        this.match(13);
                        this.betweenExpr();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        if (0 == base.inputState.guessing)
                        {
                            root = pair.root;
                            root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "BETWEEN", this.GetRelationalOperatorNodeType("BETWEEN")), root });
                            pair.root = root;
                            if ((root != null) && (null != root.getFirstChild()))
                            {
                                pair.child = root.getFirstChild();
                            }
                            else
                            {
                                pair.child = root;
                            }
                            pair.advanceChildToEnd();
                        }
                        break;

                    case 14:
                        this.match(14);
                        this.pattern();
                        if (0 == base.inputState.guessing)
                        {
                            base.astFactory.addASTChild(ref pair, base.returnAST);
                        }
                        if (0 == base.inputState.guessing)
                        {
                            root = pair.root;
                            root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "LIKE", this.GetRelationalOperatorNodeType("LIKE")), root });
                            pair.root = root;
                            if ((root != null) && (null != root.getFirstChild()))
                            {
                                pair.child = root.getFirstChild();
                            }
                            else
                            {
                                pair.child = root;
                            }
                            pair.advanceChildToEnd();
                        }
                        break;

                    default:
                        if ((this.LA(1) == 10) && (this.LA(2) == 11))
                        {
                            this.match(10);
                            this.match(11);
                            this.listInitializer();
                            if (0 == base.inputState.guessing)
                            {
                                base.astFactory.addASTChild(ref pair, base.returnAST);
                            }
                            if (0 == base.inputState.guessing)
                            {
                                root = pair.root;
                                root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "NOT", "FluorineFx.Expression.OpNOT"), (FluorineAST) base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "IN", this.GetRelationalOperatorNodeType("IN")), root }) });
                                pair.root = root;
                                if ((root != null) && (null != root.getFirstChild()))
                                {
                                    pair.child = root.getFirstChild();
                                }
                                else
                                {
                                    pair.child = root;
                                }
                                pair.advanceChildToEnd();
                            }
                        }
                        else if ((this.LA(1) == 10) && (this.LA(2) == 13))
                        {
                            this.match(10);
                            this.match(13);
                            this.betweenExpr();
                            if (0 == base.inputState.guessing)
                            {
                                base.astFactory.addASTChild(ref pair, base.returnAST);
                            }
                            if (0 == base.inputState.guessing)
                            {
                                root = pair.root;
                                root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "NOT", "FluorineFx.Expression.OpNOT"), (FluorineAST) base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "BETWEEN", this.GetRelationalOperatorNodeType("BETWEEN")), root }) });
                                pair.root = root;
                                if ((root != null) && (null != root.getFirstChild()))
                                {
                                    pair.child = root.getFirstChild();
                                }
                                else
                                {
                                    pair.child = root;
                                }
                                pair.advanceChildToEnd();
                            }
                        }
                        else
                        {
                            if ((this.LA(1) != 10) || (this.LA(2) != 14))
                            {
                                throw new NoViableAltException(this.LT(1), this.getFilename());
                            }
                            this.match(10);
                            this.match(14);
                            this.pattern();
                            if (0 == base.inputState.guessing)
                            {
                                base.astFactory.addASTChild(ref pair, base.returnAST);
                            }
                            if (0 == base.inputState.guessing)
                            {
                                root = pair.root;
                                root = base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "NOT", "FluorineFx.Expression.OpNOT"), (FluorineAST) base.astFactory.make(new AST[] { (FluorineAST) base.astFactory.create(4, "LIKE", this.GetRelationalOperatorNodeType("LIKE")), root }) });
                                pair.root = root;
                                if ((root != null) && (null != root.getFirstChild()))
                                {
                                    pair.child = root.getFirstChild();
                                }
                                else
                                {
                                    pair.child = root;
                                }
                                pair.advanceChildToEnd();
                            }
                        }
                        break;
                }
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_3_);
            }
            base.returnAST = root;
        }

        public void relationalOperator()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                switch (this.LA(1))
                {
                    case 0x20:
                    {
                        FluorineAST east2 = null;
                        east2 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east2);
                        this.match(0x20);
                        root = pair.root;
                        goto Label_028B;
                    }
                    case 0x21:
                    {
                        FluorineAST east3 = null;
                        east3 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east3);
                        this.match(0x21);
                        root = pair.root;
                        goto Label_028B;
                    }
                    case 0x22:
                    {
                        FluorineAST east4 = null;
                        east4 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east4);
                        this.match(0x22);
                        root = pair.root;
                        goto Label_028B;
                    }
                    case 0x23:
                    {
                        FluorineAST east5 = null;
                        east5 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east5);
                        this.match(0x23);
                        root = pair.root;
                        goto Label_028B;
                    }
                    case 0x24:
                    {
                        FluorineAST east6 = null;
                        east6 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east6);
                        this.match(0x24);
                        root = pair.root;
                        goto Label_028B;
                    }
                    case 0x25:
                    {
                        FluorineAST east7 = null;
                        east7 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east7);
                        this.match(0x25);
                        root = pair.root;
                        goto Label_028B;
                    }
                    case 12:
                    {
                        FluorineAST east8 = null;
                        east8 = base.astFactory.create(this.LT(1));
                        base.astFactory.addASTChild(ref pair, east8);
                        this.match(12);
                        root = pair.root;
                        goto Label_028B;
                    }
                }
                throw new NoViableAltException(this.LT(1), this.getFilename());
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_5_);
            }
        Label_028B:
            base.returnAST = root;
        }

        public override void reportError(RecognitionException ex)
        {
            throw ex;
        }

        public override void reportError(string error)
        {
            throw new RecognitionException(error);
        }

        public void sumExpr()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST east = null;
            try
            {
                bool flag;
                this.prodExpr();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
                goto Label_0163;
            Label_004D:
                if ((this.LA(1) != 0x10) && (this.LA(1) != 0x11))
                {
                    goto Label_016B;
                }
                if (this.LA(1) == 0x10)
                {
                    OpADD padd = null;
                    padd = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpADD");
                    base.astFactory.makeASTRoot(ref pair, padd);
                    this.match(0x10);
                }
                else
                {
                    if (this.LA(1) != 0x11)
                    {
                        throw new NoViableAltException(this.LT(1), this.getFilename());
                    }
                    OpSUBTRACT psubtract = null;
                    psubtract = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpSUBTRACT");
                    base.astFactory.makeASTRoot(ref pair, psubtract);
                    this.match(0x11);
                }
                this.prodExpr();
                if (0 == base.inputState.guessing)
                {
                    base.astFactory.addASTChild(ref pair, base.returnAST);
                }
            Label_0163:
                flag = true;
                goto Label_004D;
            Label_016B:
                east = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_4_);
            }
            base.returnAST = east;
        }

        public void unaryExpression()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                if (((this.LA(1) == 10) || (this.LA(1) == 0x10)) || (this.LA(1) == 0x11))
                {
                    switch (this.LA(1))
                    {
                        case 0x10:
                        {
                            OpUnaryPlus plus = null;
                            plus = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpUnaryPlus");
                            base.astFactory.makeASTRoot(ref pair, plus);
                            this.match(0x10);
                            break;
                        }
                        case 0x11:
                        {
                            OpUnaryMinus minus = null;
                            minus = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpUnaryMinus");
                            base.astFactory.makeASTRoot(ref pair, minus);
                            this.match(0x11);
                            break;
                        }
                        case 10:
                        {
                            OpNOT pnot = null;
                            pnot = base.astFactory.create(this.LT(1), "FluorineFx.Expression.OpNOT");
                            base.astFactory.makeASTRoot(ref pair, pnot);
                            this.match(10);
                            break;
                        }
                        default:
                            throw new NoViableAltException(this.LT(1), this.getFilename());
                    }
                    this.unaryExpression();
                    if (0 == base.inputState.guessing)
                    {
                        base.astFactory.addASTChild(ref pair, base.returnAST);
                    }
                    root = pair.root;
                }
                else
                {
                    if (!tokenSet_8_.member(this.LA(1)))
                    {
                        throw new NoViableAltException(this.LT(1), this.getFilename());
                    }
                    this.primaryExpression();
                    if (0 == base.inputState.guessing)
                    {
                        base.astFactory.addASTChild(ref pair, base.returnAST);
                    }
                    root = pair.root;
                }
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }

        public void var()
        {
            base.returnAST = null;
            ASTPair pair = new ASTPair();
            FluorineAST root = null;
            try
            {
                VariableNode node = null;
                node = base.astFactory.create(this.LT(1), "FluorineFx.Expression.VariableNode");
                base.astFactory.makeASTRoot(ref pair, node);
                this.match(0x19);
                root = pair.root;
            }
            catch (RecognitionException exception)
            {
                if (0 != base.inputState.guessing)
                {
                    throw exception;
                }
                this.reportError(exception);
                this.recover(exception, tokenSet_9_);
            }
            base.returnAST = root;
        }
    }
}

