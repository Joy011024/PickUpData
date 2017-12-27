namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class LinkedList : IList, ICollection, IEnumerable
    {
        private int _modId;
        private int _nodeIndex;
        private Node _rootNode;

        public LinkedList()
        {
            this._rootNode = new Node(null, null, null);
            this._rootNode.PreviousNode = this._rootNode;
            this._rootNode.NextNode = this._rootNode;
        }

        public LinkedList(IList list) : this()
        {
            this.AddAll(list);
        }

        public int Add(object value)
        {
            this.Insert(this._nodeIndex, value);
            return (this._nodeIndex - 1);
        }

        public void AddAll(IList elements)
        {
            foreach (object obj2 in elements)
            {
                this.Add(obj2);
            }
        }

        private void CheckUpdateState()
        {
            if (this.IsReadOnly || this.IsFixedSize)
            {
                throw new NotSupportedException("LinkedList cannot be modified.");
            }
        }

        public void Clear()
        {
            this._rootNode = new Node(null, null, null);
            this._rootNode.PreviousNode = this._rootNode;
            this._rootNode.NextNode = this._rootNode;
            this._nodeIndex = 0;
            this._modId++;
        }

        public bool Contains(object value)
        {
            return (this.GetNode(value) != null);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((index < 0) || (index > array.Length))
            {
                throw new ArgumentOutOfRangeException("index", string.Format("Index {0} is out of range.", index));
            }
            if ((array.Length - index) < this._nodeIndex)
            {
                throw new ArgumentException("Array is of insufficient size.");
            }
            Node nextNode = this._rootNode;
            int num = 0;
            for (int i = index; num < this._nodeIndex; i++)
            {
                nextNode = nextNode.NextNode;
                array.SetValue(nextNode.Value, i);
                num++;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new LinkedListEnumerator(this);
        }

        private Node GetNode(int index)
        {
            this.ValidateIndex(index);
            Node nextNode = this._rootNode;
            for (int i = 0; i <= index; i++)
            {
                nextNode = nextNode.NextNode;
            }
            return nextNode;
        }

        private NodeHolder GetNode(object value)
        {
            Node node;
            int index = 0;
            if (value == null)
            {
                for (node = this._rootNode.NextNode; node != this._rootNode; node = node.NextNode)
                {
                    if (node.Value == null)
                    {
                        return new NodeHolder(node, index);
                    }
                    index++;
                }
            }
            else
            {
                for (node = this._rootNode.NextNode; node != this._rootNode; node = node.NextNode)
                {
                    if (value.Equals(node.Value))
                    {
                        return new NodeHolder(node, index);
                    }
                    index++;
                }
            }
            return null;
        }

        public int IndexOf(object value)
        {
            NodeHolder node = this.GetNode(value);
            if (node == null)
            {
                return -1;
            }
            return node.Index;
        }

        public void Insert(int index, object value)
        {
            this.CheckUpdateState();
            Node node = null;
            if (index == this._nodeIndex)
            {
                node = new Node(value, this._rootNode.PreviousNode, this._rootNode);
            }
            else
            {
                Node next = this.GetNode(index);
                node = new Node(value, next.PreviousNode, next);
            }
            node.PreviousNode.NextNode = node;
            node.NextNode.PreviousNode = node;
            this._nodeIndex++;
            this._modId++;
        }

        public void Remove(object value)
        {
            this.CheckUpdateState();
            NodeHolder node = this.GetNode(value);
            this.RemoveNode(node.Node);
        }

        public void RemoveAt(int index)
        {
            this.CheckUpdateState();
            this.RemoveNode(this.GetNode(index));
        }

        private void RemoveNode(Node node)
        {
            Node previousNode = node.PreviousNode;
            previousNode.NextNode = node.NextNode;
            node.NextNode.PreviousNode = previousNode;
            node.PreviousNode = null;
            node.NextNode = null;
            this._nodeIndex--;
            this._modId++;
        }

        private void ValidateIndex(int index)
        {
            if ((index < 0) || (index >= this._nodeIndex))
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        public int Count
        {
            get
            {
                return this._nodeIndex;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object this[int index]
        {
            get
            {
                return this.GetNode(index).Value;
            }
            set
            {
                this.GetNode(index).Value = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        private class LinkedListEnumerator : IEnumerator
        {
            private LinkedList.Node _current;
            private LinkedList _ll;
            private int _modId;

            public LinkedListEnumerator(LinkedList ll)
            {
                this._ll = ll;
                this._modId = ll._modId;
                this._current = this._ll._rootNode;
            }

            public bool MoveNext()
            {
                if (this._modId != this._ll._modId)
                {
                    throw new InvalidOperationException("LinkedList has been modified.");
                }
                this._current = this._current.NextNode;
                return (this._current != this._ll._rootNode);
            }

            public void Reset()
            {
                this._current = this._ll._rootNode;
            }

            public object Current
            {
                get
                {
                    return this._current.Value;
                }
            }
        }

        [Serializable]
        private class Node
        {
            private LinkedList.Node _next;
            private LinkedList.Node _previous;
            private object _value;

            public Node(object val, LinkedList.Node previous, LinkedList.Node next)
            {
                this._value = val;
                this._next = next;
                this._previous = previous;
            }

            public LinkedList.Node NextNode
            {
                get
                {
                    return this._next;
                }
                set
                {
                    this._next = value;
                }
            }

            public LinkedList.Node PreviousNode
            {
                get
                {
                    return this._previous;
                }
                set
                {
                    this._previous = value;
                }
            }

            public object Value
            {
                get
                {
                    return this._value;
                }
                set
                {
                    this._value = value;
                }
            }
        }

        private class NodeHolder
        {
            private int _index;
            private FluorineFx.Collections.LinkedList.Node _node;

            public NodeHolder(FluorineFx.Collections.LinkedList.Node node, int index)
            {
                this._node = node;
                this._index = index;
            }

            public int Index
            {
                get
                {
                    return this._index;
                }
            }

            public FluorineFx.Collections.LinkedList.Node Node
            {
                get
                {
                    return this._node;
                }
            }
        }
    }
}

