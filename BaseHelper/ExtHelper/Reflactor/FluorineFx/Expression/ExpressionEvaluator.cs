namespace FluorineFx.Expression
{
    using System;
    using System.Collections;

    public class ExpressionEvaluator
    {
        public static object Evaluate(object root, string expression, IDictionary variables)
        {
            return FluorineFx.Expression.Expression.Parse(expression).Evaluate(root, variables);
        }
    }
}

