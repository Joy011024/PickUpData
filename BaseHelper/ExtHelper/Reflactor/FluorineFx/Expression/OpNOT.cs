namespace FluorineFx.Expression
{
    using System;

    internal class OpNOT : UnaryOperator
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            return !Convert.ToBoolean(base.Operand.EvaluateInternal(context, evalContext));
        }
    }
}

