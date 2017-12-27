namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpUnaryPlus : UnaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object number = base.Operand.EvaluateInternal(context, evalContext);
            if (!NumberUtils.IsNumber(number))
            {
                throw new ArgumentException("Specified operand is not a number. Only numbers support unary minus operator.");
            }
            return number;
        }
    }
}

