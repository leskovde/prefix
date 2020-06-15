using System;
using System.IO;

namespace Expression
{
    internal enum Errors
    {
        Format,
        Overflow,
        Divide
    }

    internal enum Commands
    {
        Integer,
        Double,
        End,
        Expression,
        Error,
        EmptyLine,
        FullParentheses,
        MinParentheses
    }

    internal class ErrorHandler
    {
        public static void PrintError(Errors error)
        {
            Console.WriteLine(error + " Error");
        }
    }

    internal class InputProcessor
    {
        private readonly TextReader _reader;
        private string[] _subExpressions;

        public InputProcessor(TextReader reader)
        {
            _reader = reader;
        }

        public Commands GetAllSubExpressions()
        {
            var line = _reader.ReadLine();
            switch (line)
            {
                case "":
                    return Commands.EmptyLine;
                case "end":
                case null:
                    return Commands.End;
                default:
                    _subExpressions = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    switch (_subExpressions[0])
                    {
                        case "=":
                            return Commands.Expression;
                        case "i":
                            return Commands.Integer;
                        case "d":
                            return Commands.Double;
                        case "p":
                            return Commands.FullParentheses;
                        case "P":
                            return Commands.MinParentheses;
                        default:
                            return Commands.Error;
                    }
            }
        }

        public int GetSubExpressionCount()
        {
            return _subExpressions.Length - 1;
        }

        public string GetSubExpression(int index)
        {
            return _subExpressions[index % _subExpressions.Length];
        }
    }

    internal class Program
    {
        public static TextWriter Writer = Console.Out;

        private static int Main(string[] args)
        {
            var inputProcessor = new InputProcessor(Console.In);

            var treeBuilder = new TreeBuilder();
            var tree = new Tree(null);

            Commands command;

            while ((command = inputProcessor.GetAllSubExpressions()) != Commands.End)
                switch (command)
                {
                    case Commands.EmptyLine:
                        break;

                    case Commands.Error:
                        ErrorHandler.PrintError(Errors.Format);
                        break;

                    case Commands.Expression:
                        tree.Clear();
                        treeBuilder.Clear();
                        var processingFailed = false;

                        for (var index = inputProcessor.GetSubExpressionCount(); index > 0; index--)
                        {
                            var subExpression = inputProcessor.GetSubExpression(index);
                            if (treeBuilder.Process(subExpression)) continue;
                            processingFailed = true;
                            break;
                        }

                        var root = treeBuilder.GetRoot();
                        if (root == null || processingFailed)
                        {
                            ErrorHandler.PrintError(Errors.Format);
                            break;
                        }

                        tree = new Tree(root);
                        break;

                    case Commands.Integer:
                        if (tree.Root == null)
                        {
                            Program.Writer.WriteLine("Expression Missing");
                            break;
                        }

                        var intVisitor = new IntegerVisitor();
                        tree.Root.Accept(intVisitor);

                        if (intVisitor.Result == null) break;

                        Program.Writer.WriteLine(intVisitor.Result);
                        break;

                    case Commands.Double:
                        if (tree.Root == null)
                        {
                            Program.Writer.WriteLine("Expression Missing");
                            break;
                        }

                        var doubleVisitor = new DoubleVisitor();
                        tree.Root.Accept(doubleVisitor);

                        Program.Writer.WriteLine(doubleVisitor.Result.ToString("f05"));
                        break;

                    case Commands.FullParentheses:
                        if (tree.Root == null)
                        {
                            Program.Writer.WriteLine("Expression Missing");
                            break;
                        }

                        var fullParenthesesVisitor = new FullParenthesesVisitor();
                        tree.Root.Accept(fullParenthesesVisitor);

                        Program.Writer.WriteLine(fullParenthesesVisitor.ParenthesizedExpression.ToString());
                        break;

                    case Commands.MinParentheses:
                        if (tree.Root == null)
                        {
                            Program.Writer.WriteLine("Expression Missing");
                            break;
                        }

                        var minParenthesesVisitor = new MinParenthesesVisitor();
                        tree.Root.Accept(minParenthesesVisitor);

                        Program.Writer.WriteLine(minParenthesesVisitor.ParenthesizedExpression.ToString());
                        break;
                }

            return 0;
        }
    }
}