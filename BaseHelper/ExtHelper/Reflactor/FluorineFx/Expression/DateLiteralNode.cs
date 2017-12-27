namespace FluorineFx.Expression
{
    using antlr.collections;
    using System;
    using System.Globalization;

    internal class DateLiteralNode : BaseNode
    {
        private object _value;

        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            if (this._value == null)
            {
                lock (this)
                {
                    if (this._value == null)
                    {
                        AST ast = this.getFirstChild();
                        if (base.getNumberOfChildren() == 2)
                        {
                            this._value = DateTime.ParseExact(ast.getText(), ast.getNextSibling().getText(), CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            this._value = DateTime.Parse(ast.getText());
                        }
                    }
                }
            }
            return this._value;
        }
    }
}

