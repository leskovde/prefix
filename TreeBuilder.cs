using System.Collections.Generic;

namespace Expression
{
    internal class Tree
    {
        public Node Root { get; private set; }

        public Tree(Node root)
        {
            Root = root;
        }

        public void Clear()
        {
            Root = null;
        }

    }

    internal class TreeBuilder
    {
        private readonly Dictionary<Nodes, NodeFactory> _factories;
        private readonly Stack<Node> _stack;

        public TreeBuilder()
        {
            _stack = new Stack<Node>();
            _factories = new Dictionary<Nodes, NodeFactory>
            {
                {Nodes.Operand, new OperandFactory()},
                {Nodes.PlusOperator, new PlusOperatorFactory()},
                {Nodes.MinusOperator, new MinusOperatorFactory()},
                {Nodes.MultiplyOperator, new MultiplyOperatorFactory()},
                {Nodes.DivideOperator, new DivideOperatorFactory()},
                {Nodes.UnaryOperator, new UnaryOperatorFactory()}
            };
        }

        public void Clear()
        {
            _stack.Clear();
        }

        public Node GetNode(Nodes nodeType, string subExpression)
        {
            return _factories[nodeType].Create(subExpression);
        }


        private static Node SetDependencies(BinaryOperator binaryOperator, Node leftOperand, Node rightOperand)
        {
            binaryOperator.LeftNode = leftOperand;
            binaryOperator.RightNode = rightOperand;
            return binaryOperator;
        }

        private static Node SetDependencies(UnaryOperator unaryOperator, Node operand)
        {
            unaryOperator.DependentNode = operand;
            return unaryOperator;
        }

        public bool Process(string subExpression)
        {
            Nodes nodeType;
            if ((nodeType = NodeIdentification.GetNodeType(subExpression)) == Nodes.Error) return false;

            var node = GetNode(nodeType, subExpression);

            switch (nodeType)
            {
                case Nodes.UnaryOperator when _stack.Count < 1:
                    return false;

                case Nodes.UnaryOperator:
                {
                    var operand = _stack.Pop();
                    node = SetDependencies((UnaryOperator) node, operand);
                    break;
                }

                case Nodes.PlusOperator:
                case Nodes.MinusOperator:
                case Nodes.MultiplyOperator:
                case Nodes.DivideOperator:
                {
                    if (_stack.Count < 2)
                        return false;

                    var leftOperand = _stack.Pop();
                    var rightOperand = _stack.Pop();
                    node = SetDependencies((BinaryOperator) node, leftOperand, rightOperand);
                    break;
                }
            }

            _stack.Push(node);

            return true;
        }

        public Node GetRoot()
        {
            return _stack.Count == 1 ? _stack.Peek() : null;
        }
    }
}