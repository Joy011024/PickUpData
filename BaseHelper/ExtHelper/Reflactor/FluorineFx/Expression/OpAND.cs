namespace FluorineFx.Expression
{
    using System;

    internal class OpAND : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            return (!Convert.ToBoolean(base.Left.EvaluateInternal(context, evalContext)) ? ((object) 0) : ((object) Convert.ToBoolean(base.Right.EvaluateInternal(context, evalContext))));
        }
    }
}

