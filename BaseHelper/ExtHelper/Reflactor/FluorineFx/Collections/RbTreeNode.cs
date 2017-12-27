namespace FluorineFx.Collections
{
    using System;

    public class RbTreeNode
    {
        public bool IsRed;
        public RbTreeNode Left;
        public static readonly RbTreeNode Nil = new NullNode();
        public RbTreeNode Parent;
        public RbTreeNode Right;
        public object Value;

        protected RbTreeNode()
        {
        }

        public RbTreeNode(object val) : this(val, Nil, Nil, Nil, true)
        {
        }

        public RbTreeNode(object val, RbTreeNode parent, RbTreeNode left, RbTreeNode right, bool isRed)
        {
            this.Value = val;
            this.Parent = parent;
            this.Left = left;
            this.Right = right;
            this.IsRed = isRed;
        }

        public virtual bool IsNull
        {
            get
            {
                return false;
            }
        }

        private class NullNode : RbTreeNode
        {
            public NullNode()
            {
                base.Parent = this;
                base.Left = this;
                base.Right = this;
                base.IsRed = false;
            }

            public override bool IsNull
            {
                get
                {
                    return true;
                }
            }
        }
    }
}

