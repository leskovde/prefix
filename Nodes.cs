namespace Expression
{
    internal enum Nodes
    {
        PlusOperator,
        MinusOperator,
        MultiplyOperator,
        DivideOperator,
        UnaryOperator,
        Operand,
        Error
    }

    internal interface IVisitable
    {
        void Accept(IVisitor visitor);
    }

    internal abstract class Node : IVisitable
    {
        public int Value { get; protected set; }

        public abstract void Accept(IVisitor visitor);
    }


    internal abstract class BinaryOperator : Node
    {
        public Node LeftNode, RightNode;

        protected BinaryOperator()
        {
        }

        protected BinaryOperator(Node leftNode, Node rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }

    internal class PlusOperator : BinaryOperator
    {
        public PlusOperator()
        {
        }

        public PlusOperator(Node leftNode, Node rightNode) : base(leftNode, rightNode)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class MinusOperator : BinaryOperator
    {
        public MinusOperator()
        {
        }

        public MinusOperator(Node leftNode, Node rightNode) : base(leftNode, rightNode)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class MultiplyOperator : BinaryOperator
    {
        public MultiplyOperator()
        {
        }

        public MultiplyOperator(Node leftNode, Node rightNode) : base(leftNode, rightNode)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class DivideOperator : BinaryOperator
    {
        public DivideOperator()
        {
        }

        public DivideOperator(Node leftNode, Node rightNode) : base(leftNode, rightNode)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class UnaryOperator : Node
    {
        public Node DependentNode;

        public UnaryOperator()
        {
        }

        public UnaryOperator(Node dependentNode)
        {
            DependentNode = dependentNode;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class Operand : Node
    {
        public Operand(int operand)
        {
            Value = operand;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    internal class NodeIdentification
    {
        public static Nodes GetNodeType(string subExpression)
        {
            switch (subExpression)
            {
                case "+":
                    return Nodes.PlusOperator;
                case "-":
                    return Nodes.MinusOperator;
                case "*":
                    return Nodes.MultiplyOperator;
                case "/":
                    return Nodes.DivideOperator;
                case "~":
                    return Nodes.UnaryOperator;
                default:
                    if (!int.TryParse(subExpression, out var operand)) return Nodes.Error;
                    return operand >= 0 ? Nodes.Operand : Nodes.Error;
            }
        }
    }
}