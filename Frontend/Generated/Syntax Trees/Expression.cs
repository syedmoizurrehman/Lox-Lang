/*
 * This is an automatically generated file. Any changes made will be lost upon regeneration.
*/

using System.Diagnostics;

namespace Frontend.Expressions
{
	public interface IExpressionVisitor<T>
	{
		T VisitBinaryExpression(BinaryExpression binaryExpression);
		T VisitGroupingExpression(GroupingExpression groupingExpression);
		T VisitLiteralExpression(LiteralExpression literalExpression);
		T VisitUnaryExpression(UnaryExpression unaryExpression);
	}

	public abstract class Expression
    {
		public abstract T Accept<T>(IExpressionVisitor<T> visitor);
	}

	public class BinaryExpression : Expression
    {
		public Expression Left { get; }

		public Token BinaryOperator { get; }

		public Expression Right { get; }

		[DebuggerStepThrough]
		public BinaryExpression(Expression left, Token binaryOperator, Expression right)
		{
			Left = left;
			BinaryOperator = binaryOperator;
			Right = right;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinaryExpression(this);
	}

	public class GroupingExpression : Expression
    {
		public Expression EnclosedExpression { get; }

		[DebuggerStepThrough]
		public GroupingExpression(Expression enclosedExpression)
		{
			EnclosedExpression = enclosedExpression;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitGroupingExpression(this);
	}

	public class LiteralExpression : Expression
    {
		public object Value { get; }

		[DebuggerStepThrough]
		public LiteralExpression(object value)
		{
			Value = value;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteralExpression(this);
	}

	public class UnaryExpression : Expression
    {
		public Token UnaryOperator { get; }

		public Expression Right { get; }

		[DebuggerStepThrough]
		public UnaryExpression(Token unaryOperator, Expression right)
		{
			UnaryOperator = unaryOperator;
			Right = right;
		}

		public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitUnaryExpression(this);
	}
}