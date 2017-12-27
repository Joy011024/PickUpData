namespace FluorineFx.Expression
{
    using System;

    internal class NullLiteralNode : BaseNode
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            return null;
        }
    }
}

