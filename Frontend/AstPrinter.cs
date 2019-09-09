using System.Text;

namespace Frontend.Expressions
{
    public static class AstPrinter
    {
        public static string Print(Expression e) => PrintExpression((dynamic)e);

        private static string Parenthesize(string name, params Expression[] expressions)
        {
            var Builder = new StringBuilder();

            Builder.Append('(').Append(name);
            foreach (var Expr in expressions)
                Builder.Append(" ").Append(Print(Expr));

            Builder.Append(")");

            return Builder.ToString();
        }

        private static string PrintExpression(BinaryExpression binaryExpression) => Parenthesize(binaryExpression.BinaryOperator.Lexeme, binaryExpression.Left, binaryExpression.Right);
        private static string PrintExpression(GroupingExpression groupingExpression) => Parenthesize("group", groupingExpression.EnclosedExpression);
        private static string PrintExpression(LiteralExpression literalExpression) => literalExpression.Value?.ToString() ?? "nil";
        private static string PrintExpression(UnaryExpression unaryExpression) => Parenthesize(unaryExpression.UnaryOperator.Lexeme, unaryExpression.Right);
    }
}
