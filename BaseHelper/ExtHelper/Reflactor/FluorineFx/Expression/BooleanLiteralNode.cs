namespace FluorineFx.Expression
{
    using System;

    internal class BooleanLiteralNode : BaseNode
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
                        this._value = bool.Parse(this.getText());
                    }
                }
            }
            return this._value;
        }
    }
}

