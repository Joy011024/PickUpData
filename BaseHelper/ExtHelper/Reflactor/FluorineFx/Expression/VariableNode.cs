namespace FluorineFx.Expression
{
    using System;

    internal class VariableNode : BaseNode
    {
        protected override object Evaluate(object context, BaseNode.EvaluationContext evalContext)
        {
            string str = this.getText();
            switch (str)
            {
                case "this":
                    return evalContext.ThisContext;

                case "root":
                    return evalContext.RootContext;
            }
            return evalContext.Variables[str];
        }
    }
}

