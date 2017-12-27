namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpMULTIPLY : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object number = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            if (!NumberUtils.IsNumber(number) || !NumberUtils.IsNumber(obj3))
            {
                throw new ArgumentException("Cannot multiply instances of '" + number.GetType().FullName + "' and '" + obj3.GetType().FullName + "'.");
            }
            return NumberUtils.Multiply(number, obj3);
        }
    }
}

