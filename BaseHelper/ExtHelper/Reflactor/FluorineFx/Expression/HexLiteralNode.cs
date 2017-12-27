namespace FluorineFx.Expression
{
    using System;
    using System.Globalization;

    internal class HexLiteralNode : BaseNode
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
                        string str = this.getText();
                        try
                        {
                            this._value = int.Parse(str.Substring(2), NumberStyles.HexNumber);
                        }
                        catch (OverflowException)
                        {
                            this._value = long.Parse(str.Substring(2), NumberStyles.HexNumber);
                        }
                    }
                }
            }
            return this._value;
        }
    }
}

