namespace FluorineFx.Expression
{
    using System;
    using System.Collections;
    using System.Reflection.Emit;

    internal abstract class BaseNode : FluorineAST, IExpression, IExpressionGenerator
    {
        public virtual void Emit(ILGenerator ilg)
        {
        }

        protected abstract object Evaluate(object context, EvaluationContext evalContext);
        public object Evaluate(object context, IDictionary variables)
        {
            EvaluationContext evalContext = new EvaluationContext(context, variables);
            return this.Evaluate(context, evalContext);
        }

        protected internal object EvaluateInternal(object context, EvaluationContext evalContext)
        {
            return this.Evaluate(context, evalContext);
        }

        public class EvaluationContext
        {
            public object RootContext;
            public object ThisContext;
            public IDictionary Variables;

            public EvaluationContext(object rootContext, IDictionary globalVariables)
            {
                this.RootContext = rootContext;
                this.ThisContext = rootContext;
                this.Variables = globalVariables;
            }

            public IDisposable SwitchThisContext()
            {
                return new ThisContextHolder(this);
            }

            public Type RootContextType
            {
                get
                {
                    return ((this.RootContext == null) ? null : this.RootContext.GetType());
                }
            }

            private class ThisContextHolder : IDisposable
            {
                private readonly BaseNode.EvaluationContext owner;
                private readonly object savedThisContext;

                public ThisContextHolder(BaseNode.EvaluationContext owner)
                {
                    this.owner = owner;
                    this.savedThisContext = owner.ThisContext;
                }

                public void Dispose()
                {
                    this.owner.ThisContext = this.savedThisContext;
                }
            }
        }
    }
}

