namespace FluorineFx.Expression
{
    using FluorineFx.Util;
    using System;
    using System.Collections;

    internal class OpBetween : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object first = base.Left.EvaluateInternal(context, evalContext);
            IList list = base.Right.EvaluateInternal(context, evalContext) as IList;
            if ((list == null) || (list.Count != 2))
            {
                throw new ArgumentException("Right operand for the 'between' operator has to be a two-element list.");
            }
            object second = list[0];
            object obj4 = list[1];
            return ((CompareUtils.Compare(first, second) < 0) ? ((object) 0) : ((object) (CompareUtils.Compare(first, obj4) <= 0)));
        }
    }
}

