namespace FluorineFx.Expression
{
    internal abstract class BinaryOperator : BaseNode
    {
        public BaseNode Left
        {
            get
            {
                return (BaseNode) this.getFirstChild();
            }
        }

        public BaseNode Right
        {
            get
            {
                return (BaseNode) this.getFirstChild().getNextSibling();
            }
        }
    }
}

