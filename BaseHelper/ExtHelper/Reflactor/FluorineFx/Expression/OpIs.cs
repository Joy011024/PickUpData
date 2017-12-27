namespace FluorineFx.Expression
{
    using System;

    internal class OpIs : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object obj2 = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            return (((obj2 == null) && (obj3 == null)) || ((obj2 != null) && (obj3 != null)));
        }
    }
}

