namespace FluorineFx.Expression
{
    using System;

    internal class OpOR : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            return (Convert.ToBoolean(base.Left.EvaluateInternal(context, evalContext)) ? ((object) 1) : ((object) Convert.ToBoolean(base.Right.EvaluateInternal(context, evalContext))));
        }
    }
}

