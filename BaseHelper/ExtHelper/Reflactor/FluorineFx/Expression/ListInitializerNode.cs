namespace FluorineFx.Expression
{
    using System;
    using System.Collections;

    internal class ListInitializerNode : NodeWithArguments
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            return new ArrayList(base.ResolveArguments(evalContext));
        }
    }
}

