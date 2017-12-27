namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;

    internal class OpEqual : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object first = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            if (first == null)
            {
                return (obj3 == null);
            }
            if (obj3 == null)
            {
                return false;
            }
            if (first.GetType() == obj3.GetType())
            {
                return first.Equals(obj3);
            }
            if (first.GetType().IsEnum && (obj3 is string))
            {
                return first.Equals(Enum.Parse(first.GetType(), (string) obj3));
            }
            if (obj3.GetType().IsEnum && (first is string))
            {
                return obj3.Equals(Enum.Parse(obj3.GetType(), (string) first));
            }
            return (CompareUtils.Compare(first, obj3) == 0);
        }
    }
}

