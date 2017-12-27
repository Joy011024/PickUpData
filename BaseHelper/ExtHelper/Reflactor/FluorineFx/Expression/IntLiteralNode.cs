namespace FluorineFx.Expression
{
    using System;

    internal class IntLiteralNode : BaseNode
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
                        string s = this.getText();
                        try
                        {
                            this._value = int.Parse(s);
                        }
                        catch (OverflowException)
                        {
                            this._value = long.Parse(s);
                        }
                    }
                }
            }
            return this._value;
        }
    }
}

