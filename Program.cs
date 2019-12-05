using System;
using System.IO;
using System.Text;

namespace Expression
{
    public enum Nodes
    {
        Operator,
        Operand
    }
    internal interface INode
    {
        int GetValue();
    }

    internal class Operator : INode
    {
        private char _operator;

        public Operator(string @operator)
        {
            _operator = @operator[0];
        }

        public int GetValue()
        {
            return _operator;
        }
    }

    internal class Operand : INode
    {
        private int _operand;

        public Operand(string operand)
        {
            if (Int32.TryParse(operand, out int parsedOperand))
                _operand = parsedOperand;
        }

        public int GetValue()
        {
            return _operand;
        }
    }

    internal abstract class NodeFactory
    {
        public abstract INode Create(string subExpression);
    }

    internal class OperatorFactory : NodeFactory
    {
        public override INode Create(string subExpression) => new Operator(subExpression);
    }

    internal class OperandFactory : NodeFactory
    {
        public override INode Create(string subExpression) => new Operator(subExpression);
    }

    class Trash
    {
        private bool IsOperator(string subExpression)
        {
            switch (subExpression)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "~":
                    return true;
                default:
                    return false;
            }
        }

        public INode GetNode(string subExpression)
        {
            if (IsOperator(subExpression))
                return new Operator(subExpression);

            if (Int32.TryParse(subExpression, out int operand))
                return new Operand(subExpression);
            else
                return null;
        }
    }

    internal class InputProcessor
    {
        private readonly TextReader _reader;

        public InputProcessor(TextReader reader)
        {
            _reader = reader;
        }

        private bool IsSeparator(int character)
        {
            switch (character)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                case -1:
                    return true;
                default:
                    return false;
            }
        }

        public string GetSubExpression()
        {
            var subExpression = new StringBuilder();

            while (!IsSeparator(_reader.Peek()))
            {
                subExpression.Append(_reader.Read());
            }

            return subExpression.ToString();
        }
    }

    internal class InputProcessorUsingSplit
    {
        private readonly TextReader _reader;
        private string[] _subExpressions;
        public InputProcessorUsingSplit(TextReader reader)
        {
            _reader = reader;
            GetAllSubExpressions();
        }

        private void GetAllSubExpressions()
        {
            _subExpressions = _reader.ReadToEnd().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        }

        public int GetSubExpressionCount()
        {
            return _subExpressions.Length;
        }
        public string GetSubExpression(int index)
        {
            return _subExpressions[index % _subExpressions.Length];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var inputProcessor = new InputProcessor(Console.In);
            var inputProcessorUsingSplit = new InputProcessorUsingSplit(Console.In);

            string subexpression;
            while ((subexpression = inputProcessor.GetSubExpression()) != "")
            {
                var node = NodeFactory.Get
            }
        }
    }
}
