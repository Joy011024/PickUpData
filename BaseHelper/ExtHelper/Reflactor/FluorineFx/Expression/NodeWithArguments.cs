namespace FluorineFx.Expression
{
    using antlr.collections;
    using System;
    using System.Collections;

    internal abstract class NodeWithArguments : BaseNode
    {
        private BaseNode[] args;

        protected void AssertArgumentCount(int requiredCount)
        {
            this.InitializeNode();
            if (requiredCount != this.args.Length)
            {
                throw new ArgumentException(string.Concat(new object[] { "This expression node requires exactly ", requiredCount, " argument(s) and ", this.args.Length, " were specified." }));
            }
        }

        private void InitializeNode()
        {
            if (this.args == null)
            {
                lock (this)
                {
                    if (this.args == null)
                    {
                        ArrayList list = new ArrayList();
                        for (AST ast = this.getFirstChild(); ast != null; ast = ast.getNextSibling())
                        {
                            list.Add(ast);
                        }
                        this.args = (BaseNode[]) list.ToArray(typeof(BaseNode));
                    }
                }
            }
        }

        protected object ResolveArgument(int position, BaseNode.EvaluationContext evalContext)
        {
            this.InitializeNode();
            return this.args[position].EvaluateInternal(evalContext.ThisContext, evalContext);
        }

        protected object[] ResolveArguments(BaseNode.EvaluationContext evalContext)
        {
            this.InitializeNode();
            object[] objArray = new object[this.args.Length];
            for (int i = 0; i < this.args.Length; i++)
            {
                objArray[i] = this.ResolveArgument(i, evalContext);
            }
            return objArray;
        }
    }
}

