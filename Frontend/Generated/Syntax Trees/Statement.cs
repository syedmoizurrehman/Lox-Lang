/*
 * This is an automatically generated file. Any changes made will be lost upon regeneration.
*/

using Frontend.Expressions;
using System.Diagnostics;

namespace Frontend.Statements
{
	public abstract class Statement
    {
	}

	public class ExpressionStatement : Statement
    {
		public Expression Expr { get; }

		[DebuggerStepThrough]
		public ExpressionStatement(Expression expr)
		{
			Expr = expr;
		}
	}

	public class PrintStatement : Statement
    {
		public Expression Expr { get; }

		[DebuggerStepThrough]
		public PrintStatement(Expression expr)
		{
			Expr = expr;
		}
	}
}