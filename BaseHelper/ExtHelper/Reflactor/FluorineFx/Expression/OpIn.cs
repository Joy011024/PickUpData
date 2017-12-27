namespace FluorineFx.Expression
{
    using System;
    using System.Collections;

    internal class OpIn : BinaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            object obj2 = base.Left.EvaluateInternal(context, evalContext);
            object obj3 = base.Right.EvaluateInternal(context, evalContext);
            if (obj3 == null)
            {
                return false;
            }
            if (obj3 is IList)
            {
                return ((IList) obj3).Contains(obj2);
            }
            if (!(obj3 is IDictionary))
            {
                throw new ArgumentException("Right hand parameter for 'in' operator has to be an instance of IList or IDictionary.");
            }
            return ((IDictionary) obj3).Contains(obj2);
        }
    }
}

