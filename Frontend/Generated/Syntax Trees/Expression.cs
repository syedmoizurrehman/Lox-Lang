/*
 * This is an automatically generated file. Any changes made will be lost upon regeneration.
*/

using System.Diagnostics;

namespace Frontend.Expressions
{
	public abstract class Expression
    {
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
	}

	public class GroupingExpression : Expression
    {
		public Expression EnclosedExpression { get; }

		[DebuggerStepThrough]
		public GroupingExpression(Expression enclosedExpression)
		{
			EnclosedExpression = enclosedExpression;
		}
	}

	public class LiteralExpression : Expression
    {
		public object Value { get; }

		[DebuggerStepThrough]
		public LiteralExpression(object value)
		{
			Value = value;
		}
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
	}
}