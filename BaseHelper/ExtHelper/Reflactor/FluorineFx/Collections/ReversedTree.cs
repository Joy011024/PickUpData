namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    internal sealed class ReversedTree : IEnumerable
    {
        private RbTree _tree;

        public ReversedTree(RbTree tree)
        {
            this._tree = tree;
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this._tree);
        }

        private sealed class Enumerator : IEnumerator
        {
            private RbTreeNode _currentNode;
            private RbTree _tree;

            public Enumerator(RbTree tree)
            {
                this._tree = tree;
            }

            public bool MoveNext()
            {
                if (this._currentNode == null)
                {
                    this._currentNode = this._tree.First;
                }
                else
                {
                    this._currentNode = this._tree.Next(this._currentNode);
                }
                return !this._currentNode.IsNull;
            }

            public void Reset()
            {
                this._currentNode = null;
            }

            public object Current
            {
                get
                {
                    return this._currentNode.Value;
                }
            }
        }
    }
}

