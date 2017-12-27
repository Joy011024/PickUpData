namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpLess : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object first = base.Left.EvaluateInternal(context, evalContext);
            object second = base.Right.EvaluateInternal(context, evalContext);
            return (CompareUtils.Compare(first, second) < 0);
        }
    }
}

