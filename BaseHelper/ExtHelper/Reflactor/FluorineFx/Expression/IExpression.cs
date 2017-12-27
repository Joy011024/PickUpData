namespace FluorineFx.Expression
{
    using System;
    using System.Collections;

    internal interface IExpression
    {
        object Evaluate(object context, IDictionary variables);
    }
}

