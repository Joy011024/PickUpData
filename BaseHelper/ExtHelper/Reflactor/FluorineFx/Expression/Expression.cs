namespace FluorineFx.Expression
{
    using antlr;
    using antlr.collections;
    using System;
    using System.Collections;
    using System.IO;

    internal class Expression : BaseNode
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object obj2 = context;
            if (base.getNumberOfChildren() > 0)
            {
                for (AST ast = this.getFirstChild(); ast != null; ast = ast.getNextSibling())
                {
                    obj2 = ((BaseNode) ast).EvaluateInternal(obj2, evalContext);
                }
            }
            return obj2;
        }

        public static IExpression Parse(string expression)
        {
            if ((expression != null) && (expression != string.Empty))
            {
                ExpressionLexer lexer = new ExpressionLexer(new StringReader(expression));
                ExpressionParser parser = new FluorineExpressionParser(lexer);
                parser.expr();
                return (parser.getAST() as IExpression);
            }
            return new FluorineFx.Expression.Expression();
        }

        private class FluorineASTFactory : ASTFactory
        {
            public FluorineASTFactory(Type t) : base(t.FullName)
            {
                base.defaultASTNodeTypeObject_ = t;
                base.typename2creator_ = new Hashtable(0x20, 0.3f);
                base.typename2creator_[t.FullName] = FluorineAST.Creator;
            }
        }

        private class FluorineExpressionParser : ExpressionParser
        {
            public FluorineExpressionParser(TokenStream lexer) : base(lexer)
            {
                base.astFactory = new FluorineFx.Expression.Expression.FluorineASTFactory(typeof(FluorineAST));
                base.initialize();
            }
        }
    }
}

