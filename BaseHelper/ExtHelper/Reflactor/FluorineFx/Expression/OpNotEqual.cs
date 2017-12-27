namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpNotEqual : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object first = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            if (first == null)
            {
                return (obj3 != null);
            }
            if (obj3 == null)
            {
                return true;
            }
            if (first.GetType() == obj3.GetType())
            {
                return !first.Equals(obj3);
            }
            return (CompareUtils.Compare(first, obj3) != 0);
        }
    }
}

