namespace FluorineFx.Expression
{
    using System;

    internal class StringLiteralNode : BaseNode
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            return this.getText();
        }
    }
}

