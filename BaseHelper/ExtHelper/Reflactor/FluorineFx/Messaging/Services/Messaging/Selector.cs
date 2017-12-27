namespace FluorineFx.Messaging.Services.Messaging
{
    using FluorineFx;
    using FluorineFx.Expression;
    using log4net;
    using System;
    using System.Collections;

    internal class Selector
    {
        private EvaluateInvoker _evaluateMethod;
        private static readonly ILog log = LogManager.GetLogger(typeof(Selector));

        public Selector(EvaluateInvoker evaluateMethod)
        {
            this._evaluateMethod = evaluateMethod;
        }

        public static Selector CreateSelector(string expression)
        {
            IExpression expression2 = FluorineFx.Expression.Expression.Parse(expression);
            if (expression2 is IExpressionGenerator)
            {
                return new Selector(new EvaluateInvoker(expression2.Evaluate));
            }
            return new Selector(new EvaluateInvoker(expression2.Evaluate));
        }

        public bool Evaluate(object root, IDictionary variables)
        {
            object obj2 = this._evaluateMethod(root, variables);
            if (obj2 is bool)
            {
                return (bool) obj2;
            }
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Selector_InvalidResult"));
            }
            return false;
        }
    }
}

