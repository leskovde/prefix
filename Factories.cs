namespace Expression
{
    internal abstract class NodeFactory
    {
        public abstract Node Create(string subExpression);
    }

    internal class PlusOperatorFactory : NodeFactory
    {
        public override Node Create(string subExpression)
        {
            return new PlusOperator();
        }
    }

    internal class MinusOperatorFactory : NodeFactory
    {
        public override Node Create(string subExpression)
        {
            return new MinusOperator();
        }
    }

    internal class MultiplyOperatorFactory : NodeFactory
    {
        public override Node Create(string subExpression)
        {
            return new MultiplyOperator();
        }
    }

    internal class DivideOperatorFactory : NodeFactory
    {
        public override Node Create(string subExpression)
        {
            return new DivideOperator();
        }
    }

    internal class UnaryOperatorFactory : NodeFactory
    {
        public override Node Create(string subExpression)
        {
            return new UnaryOperator();
        }
    }

    internal class OperandFactory : NodeFactory
    {
        public override Node Create(string subExpression)
        {
            if (!int.TryParse(subExpression, out var parsedResult)) return null;
            return parsedResult >= 0 ? new Operand(parsedResult) : null;
        }
    }
}