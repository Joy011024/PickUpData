namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    internal class SetEnumerator : IEnumerator
    {
        private RbTreeNode _currentNode = null;
        private RbTree _tree;

        public SetEnumerator(RbTree tree)
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

