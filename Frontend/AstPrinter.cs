using System.Text;

namespace Frontend.Expressions
{
    public class AstPrinter : IExpressionVisitor<string>
    {
        public string Print(Expression expression) => expression.Accept(this);

        private string Parenthesize(string name, params Expression[] expressions)
        {
            var Builder = new StringBuilder();

            Builder.Append('(').Append(name);
            foreach (var Expr in expressions)
                Builder.Append(" ").Append(Print(Expr));

            Builder.Append(")");

            return Builder.ToString();
        }

        public string VisitBinaryExpression(BinaryExpression binaryExpression) => Parenthesize(binaryExpression.BinaryOperator.Lexeme, binaryExpression.Left, binaryExpression.Right);
        public string VisitGroupingExpression(GroupingExpression groupingExpression) => Parenthesize("group", groupingExpression.EnclosedExpression);
        public string VisitLiteralExpression(LiteralExpression literalExpression) => literalExpression.Value?.ToString() ?? "nil";
        public string VisitUnaryExpression(UnaryExpression unaryExpression) => Parenthesize(unaryExpression.UnaryOperator.Lexeme, unaryExpression.Right);
    }
}
