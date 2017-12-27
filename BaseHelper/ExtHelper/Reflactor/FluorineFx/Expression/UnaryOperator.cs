namespace FluorineFx.Expression
{
    internal abstract class UnaryOperator : BaseNode
    {
        public BaseNode Operand
        {
            get
            {
                return (BaseNode) this.getFirstChild();
            }
        }
    }
}

