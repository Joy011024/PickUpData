namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class RbTree
    {
        private IComparer _comparer;
        private RbTreeNode _root = new RbTreeNode(null);

        public RbTree(IComparer comparer)
        {
            this._root.IsRed = false;
            this._comparer = comparer;
        }

        private InsertResult BinaryInsert(RbTreeNode z, bool allowDuplicates, bool replaceIfDuplicate)
        {
            z.Left = RbTreeNode.Nil;
            z.Right = RbTreeNode.Nil;
            RbTreeNode node = this._root;
            RbTreeNode left = this._root.Left;
            while (left != RbTreeNode.Nil)
            {
                node = left;
                int num = this._comparer.Compare(left.Value, z.Value);
                if (!allowDuplicates && (num == 0))
                {
                    if (replaceIfDuplicate)
                    {
                        left.Value = z.Value;
                    }
                    return new InsertResult(false, left);
                }
                if (num > 0)
                {
                    left = left.Left;
                }
                else
                {
                    left = left.Right;
                }
            }
            z.Parent = node;
            if ((node == this._root) || (this._comparer.Compare(node.Value, z.Value) > 0))
            {
                node.Left = z;
            }
            else
            {
                node.Right = z;
            }
            z.Parent = node;
            return new InsertResult(true, z);
        }

        private void DeleteFixUp(RbTreeNode x)
        {
            RbTreeNode left = this._root.Left;
            while (!x.IsRed && (left != x))
            {
                RbTreeNode right;
                if (x == x.Parent.Left)
                {
                    right = x.Parent.Right;
                    if (right.IsRed)
                    {
                        right.IsRed = false;
                        x.Parent.IsRed = true;
                        this.LeftRotate(x.Parent);
                        right = x.Parent.Right;
                    }
                    if (!(right.Right.IsRed || right.Left.IsRed))
                    {
                        right.IsRed = true;
                        x = x.Parent;
                    }
                    else
                    {
                        if (!right.Right.IsRed)
                        {
                            right.Left.IsRed = false;
                            right.IsRed = true;
                            RightRotate(right);
                            right = x.Parent.Right;
                        }
                        right.IsRed = x.Parent.IsRed;
                        x.Parent.IsRed = false;
                        right.Right.IsRed = false;
                        this.LeftRotate(x.Parent);
                        x = left;
                    }
                }
                else
                {
                    right = x.Parent.Left;
                    if (right.IsRed)
                    {
                        right.IsRed = false;
                        x.Parent.IsRed = true;
                        RightRotate(x.Parent);
                        right = x.Parent.Left;
                    }
                    if (!(right.Right.IsRed || right.Left.IsRed))
                    {
                        right.IsRed = true;
                        x = x.Parent;
                    }
                    else
                    {
                        if (!right.Left.IsRed)
                        {
                            right.Right.IsRed = false;
                            right.IsRed = true;
                            this.LeftRotate(right);
                            right = x.Parent.Left;
                        }
                        right.IsRed = x.Parent.IsRed;
                        x.Parent.IsRed = false;
                        right.Left.IsRed = false;
                        RightRotate(x.Parent);
                        x = left;
                    }
                }
            }
            x.IsRed = false;
        }

        public void Erase(RbTreeNode z)
        {
            RbTreeNode node;
            RbTreeNode node3 = this._root;
            if ((z.Left == RbTreeNode.Nil) || (z.Right == RbTreeNode.Nil))
            {
                node = z;
            }
            else
            {
                node = this.Next(z);
            }
            RbTreeNode x = (node.Left == RbTreeNode.Nil) ? node.Right : node.Left;
            if (this._root == (x.Parent = node.Parent))
            {
                this._root.Left = x;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = x;
            }
            else
            {
                node.Parent.Right = x;
            }
            if (!node.IsRed)
            {
                this.DeleteFixUp(x);
            }
            if (node != z)
            {
                node.Left = z.Left;
                node.Right = z.Right;
                node.Parent = z.Parent;
                node.IsRed = z.IsRed;
                z.Left.Parent = z.Right.Parent = node;
                if (z == z.Parent.Left)
                {
                    z.Parent.Left = node;
                }
                else
                {
                    z.Parent.Right = node;
                }
            }
            IDisposable disposable = z.Value as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public int Erase(object val)
        {
            RbTreeNode x = this.LowerBound(val);
            RbTreeNode node2 = this.UpperBound(val);
            int num = 0;
            while (x != node2)
            {
                RbTreeNode z = x;
                x = this.Next(x);
                this.Erase(z);
                num++;
            }
            return num;
        }

        public InsertResult Insert(object val, bool allowDuplicates, bool replaceIfDuplicate)
        {
            RbTreeNode z = new RbTreeNode(val);
            InsertResult result = this.BinaryInsert(z, allowDuplicates, replaceIfDuplicate);
            if (!result.NewNode)
            {
                return result;
            }
            RbTreeNode x = z;
            x.IsRed = true;
            while (x.Parent.IsRed)
            {
                RbTreeNode right;
                if (x.Parent == x.Parent.Parent.Left)
                {
                    right = x.Parent.Parent.Right;
                    if (right.IsRed)
                    {
                        x.Parent.IsRed = false;
                        right.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Right)
                        {
                            x = x.Parent;
                            this.LeftRotate(x);
                        }
                        x.Parent.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        RightRotate(x.Parent.Parent);
                    }
                }
                else
                {
                    right = x.Parent.Parent.Left;
                    if (right.IsRed)
                    {
                        x.Parent.IsRed = false;
                        right.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Left)
                        {
                            x = x.Parent;
                            RightRotate(x);
                        }
                        x.Parent.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        this.LeftRotate(x.Parent.Parent);
                    }
                }
            }
            this._root.Left.IsRed = false;
            return new InsertResult(true, z);
        }

        private void LeftRotate(RbTreeNode x)
        {
            RbTreeNode right = x.Right;
            x.Right = right.Left;
            if (right.Left != RbTreeNode.Nil)
            {
                right.Left.Parent = x;
            }
            right.Parent = x.Parent;
            if (x == x.Parent.Left)
            {
                x.Parent.Left = right;
            }
            else
            {
                x.Parent.Right = right;
            }
            right.Left = x;
            x.Parent = right;
        }

        public RbTreeNode LowerBound(object val)
        {
            RbTreeNode root = this.Root;
            RbTreeNode nil = RbTreeNode.Nil;
            while (root != RbTreeNode.Nil)
            {
                if (this._comparer.Compare(val, root.Value) <= 0)
                {
                    nil = root;
                    root = root.Left;
                }
                else
                {
                    root = root.Right;
                }
            }
            return nil;
        }

        public RbTreeNode Next(RbTreeNode x)
        {
            RbTreeNode node = this._root;
            RbTreeNode right = x.Right;
            if (right != RbTreeNode.Nil)
            {
                while (right.Left != RbTreeNode.Nil)
                {
                    right = right.Left;
                }
                return right;
            }
            right = x.Parent;
            while (x == right.Right)
            {
                x = right;
                right = right.Parent;
            }
            if (right == node)
            {
                return RbTreeNode.Nil;
            }
            return right;
        }

        public RbTreeNode Prev(RbTreeNode x)
        {
            RbTreeNode node = this._root;
            RbTreeNode left = x.Left;
            if (left != RbTreeNode.Nil)
            {
                while (left.Right != RbTreeNode.Nil)
                {
                    left = left.Right;
                }
                return left;
            }
            left = x.Parent;
            while (x == left.Left)
            {
                if (left == node)
                {
                    return RbTreeNode.Nil;
                }
                x = left;
                left = left.Parent;
            }
            return left;
        }

        private static void RightRotate(RbTreeNode y)
        {
            RbTreeNode left = y.Left;
            y.Left = left.Right;
            if (left.Right != RbTreeNode.Nil)
            {
                left.Right.Parent = y;
            }
            left.Parent = y.Parent;
            if (y == y.Parent.Left)
            {
                y.Parent.Left = left;
            }
            else
            {
                y.Parent.Right = left;
            }
            left.Right = y;
            y.Parent = left;
        }

        public RbTreeNode UpperBound(object val)
        {
            RbTreeNode root = this.Root;
            RbTreeNode nil = RbTreeNode.Nil;
            while (root != RbTreeNode.Nil)
            {
                if (this._comparer.Compare(val, root.Value) < 0)
                {
                    nil = root;
                    root = root.Left;
                }
                else
                {
                    root = root.Right;
                }
            }
            return nil;
        }

        public IComparer Comparer
        {
            get
            {
                return this._comparer;
            }
        }

        public RbTreeNode First
        {
            get
            {
                RbTreeNode root = this.Root;
                while (root.Left != RbTreeNode.Nil)
                {
                    root = root.Left;
                }
                return root;
            }
        }

        public RbTreeNode Last
        {
            get
            {
                RbTreeNode root = this.Root;
                while (root.Right != RbTreeNode.Nil)
                {
                    root = root.Right;
                }
                return root;
            }
        }

        public RbTreeNode Root
        {
            get
            {
                return this._root.Left;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InsertResult
        {
            public bool NewNode;
            public RbTreeNode Node;
            public InsertResult(bool newNode, RbTreeNode node)
            {
                this.NewNode = newNode;
                this.Node = node;
            }
        }
    }
}

