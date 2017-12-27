namespace FluorineFx.Expression
{
    using System;

    public class ExpressionParserTokenTypes
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
        public const int TRUE = 7;
        public const int WS = 0x26;
    }
}

