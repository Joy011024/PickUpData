namespace FluorineFx.Expression
{
    using System;
    using System.Globalization;

    internal class RealLiteralNode : BaseNode
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
                        char c = s.ToLower()[s.Length - 1];
                        if (char.IsDigit(c))
                        {
                            this._value = double.Parse(s, NumberFormatInfo.InvariantInfo);
                        }
                        else
                        {
                            s = s.Substring(0, s.Length - 1);
                            if (c == 'm')
                            {
                                this._value = decimal.Parse(s, NumberFormatInfo.InvariantInfo);
                            }
                            else if (c == 'f')
                            {
                                this._value = float.Parse(s, NumberFormatInfo.InvariantInfo);
                            }
                            else
                            {
                                this._value = double.Parse(s, NumberFormatInfo.InvariantInfo);
                            }
                        }
                    }
                }
            }
            return this._value;
        }
    }
}

