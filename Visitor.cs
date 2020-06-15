using System;
using System.IO;
using System.Text;

namespace Expression
{
    internal interface IVisitor
    {
        void Visit(Operand operand);
        void Visit(PlusOperator @operator);
        void Visit(MinusOperator @operator);
        void Visit(MultiplyOperator @operator);
        void Visit(DivideOperator @operator);
        void Visit(UnaryOperator @operator);
    }

    internal class IntegerVisitor : IVisitor
    {
        public int? Result { get; private set; }

        public void Visit(UnaryOperator @operator)
        {
            @operator.DependentNode.Accept(this);
            if (this.Result != null)
                Result *= -1;
        }

        public void Visit(PlusOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            if (leftValue == null || rightValue == null)
                Result = null;
            else
                try
                {
                    Result = checked(leftValue + rightValue);
                }
                catch
                {
                    ErrorHandler.PrintError(Errors.Overflow);
                    Result = null;
                }
        }

        public void Visit(MinusOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            if (leftValue == null || rightValue == null)
                Result = null;
            else
                try
                {
                    Result = checked(leftValue - rightValue);
                }
                catch
                {
                    ErrorHandler.PrintError(Errors.Overflow);
                    Result = null;
                }
        }

        public void Visit(MultiplyOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            if (leftValue == null || rightValue == null)
                Result = null;
            else
                try
                {
                    Result = checked(leftValue * rightValue);
                }
                catch
                {
                    ErrorHandler.PrintError(Errors.Overflow);
                    Result = null;
                }
        }

        public void Visit(DivideOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            if (leftValue == null || rightValue == null)
                Result = null;
            else
                try
                {
                    if (rightValue == 0)
                    {
                        ErrorHandler.PrintError(Errors.Divide);
                        Result = null;
                    }
                    else
                    {
                        Result = leftValue / rightValue;
                    }
                }
                catch
                {
                    ErrorHandler.PrintError(Errors.Overflow);
                    Result = null;
                }
        }

        public void Visit(Operand operand)
        {
            Result = operand.Value;
        }
    }

    internal class DoubleVisitor : IVisitor
    {
        public double Result { get; private set; }

        public void Visit(UnaryOperator @operator)
        {
            var nodeVisitor = new DoubleVisitor();
            @operator.DependentNode.Accept(nodeVisitor);
            Result = nodeVisitor.Result * -1;
        }

        public void Visit(PlusOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result; ;

            Result = leftValue + rightValue;
        }

        public void Visit(MinusOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            Result = leftValue - rightValue;
        }

        public void Visit(MultiplyOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            Result = leftValue * rightValue;
        }

        public void Visit(DivideOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            var leftValue = this.Result;
            @operator.RightNode.Accept(this);
            var rightValue = this.Result;

            Result = leftValue / rightValue;
        }

        public void Visit(Operand operand)
        {
            Result = operand.Value;
        }
    }

    internal class FullParenthesesVisitor : IVisitor
    {
        public StringBuilder ParenthesizedExpression = new StringBuilder();

        public void Visit(UnaryOperator @operator)
        {
            ParenthesizedExpression.Append("(");
            ParenthesizedExpression.Append("-");
            @operator.DependentNode.Accept(this);
            ParenthesizedExpression.Append(")");
        }

        public void Visit(PlusOperator @operator)
        {
            ParenthesizedExpression.Append("(");
            @operator.LeftNode.Accept(this);
            ParenthesizedExpression.Append("+");
            @operator.RightNode.Accept(this);
            ParenthesizedExpression.Append(")");
        }

        public void Visit(MinusOperator @operator)
        {
            ParenthesizedExpression.Append("(");
            @operator.LeftNode.Accept(this);
            ParenthesizedExpression.Append("-");
            @operator.RightNode.Accept(this);
            ParenthesizedExpression.Append(")");
        }

        public void Visit(MultiplyOperator @operator)
        {
            ParenthesizedExpression.Append("(");
            @operator.LeftNode.Accept(this);
            ParenthesizedExpression.Append("*");
            @operator.RightNode.Accept(this);
            ParenthesizedExpression.Append(")");
        }

        public void Visit(DivideOperator @operator)
        {
            ParenthesizedExpression.Append("(");
            @operator.LeftNode.Accept(this);
            ParenthesizedExpression.Append("/");
            @operator.RightNode.Accept(this);
            ParenthesizedExpression.Append(")");
        }

        public void Visit(Operand operand)
        {
            ParenthesizedExpression.Append(operand.Value);
        }
    }

    internal class MinParenthesesVisitor : IVisitor
    {
        public StringBuilder ParenthesizedExpression = new StringBuilder();

        public void Visit(UnaryOperator @operator)
        {
            ParenthesizedExpression.Append("-");
            if (!(@operator.DependentNode is UnaryOperator|| @operator.DependentNode is Operand)) ParenthesizedExpression.Append("(");
            @operator.DependentNode.Accept(this);
            if (!(@operator.DependentNode is UnaryOperator || @operator.DependentNode is Operand)) ParenthesizedExpression.Append(")");
        }

        public void Visit(PlusOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            ParenthesizedExpression.Append("+");
            @operator.RightNode.Accept(this);
        }

        public void Visit(MinusOperator @operator)
        {
            @operator.LeftNode.Accept(this);
            ParenthesizedExpression.Append("-");
            if (@operator.RightNode is PlusOperator) ParenthesizedExpression.Append("(");
            @operator.RightNode.Accept(this);
            if (@operator.RightNode is PlusOperator) ParenthesizedExpression.Append(")");
        }

        public void Visit(MultiplyOperator @operator)
        {
            if (!(@operator.LeftNode is MultiplyOperator || @operator.LeftNode is Operand)) ParenthesizedExpression.Append("(");
            @operator.LeftNode.Accept(this);
            if (!(@operator.LeftNode is MultiplyOperator || @operator.LeftNode is Operand)) ParenthesizedExpression.Append(")");
            ParenthesizedExpression.Append("*");
            if (!(@operator.RightNode is MultiplyOperator || @operator.RightNode is Operand)) ParenthesizedExpression.Append("(");
            @operator.RightNode.Accept(this);
            if (!(@operator.RightNode is MultiplyOperator || @operator.RightNode is Operand)) ParenthesizedExpression.Append(")");
        }

        public void Visit(DivideOperator @operator)
        {
            if (!(@operator.LeftNode is DivideOperator || @operator.LeftNode is Operand)) ParenthesizedExpression.Append("(");
            @operator.LeftNode.Accept(this);
            if (!(@operator.LeftNode is DivideOperator || @operator.LeftNode is Operand)) ParenthesizedExpression.Append(")");
            ParenthesizedExpression.Append("/");
            if (!(@operator.RightNode is DivideOperator || @operator.RightNode is Operand)) ParenthesizedExpression.Append("(");
            @operator.RightNode.Accept(this);
            if (!(@operator.RightNode is DivideOperator || @operator.RightNode is Operand)) ParenthesizedExpression.Append(")");
        }

        public void Visit(Operand operand)
        {
            ParenthesizedExpression.Append(operand.Value);
        }
    }
}