/*
 * This is an automatically generated file. Any changes made will be lost upon regeneration.
*/

using Frontend.Expressions;
using System.Diagnostics;

namespace Frontend.Statements
{
	public interface IStatementVisitor<T>
	{
		T VisitExpressionStatement(ExpressionStatement expressionStatement);
		T VisitPrintStatement(PrintStatement printStatement);
	}

	public abstract class Statement
    {
        [DebuggerStepThrough]
		public abstract T Accept<T>(IStatementVisitor<T> visitor);
	}

	public class ExpressionStatement : Statement
    {
		public Expression Expr { get; }

		[DebuggerStepThrough]
		public ExpressionStatement(Expression expr)
		{
			Expr = expr;
		}

        [DebuggerStepThrough]
		public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitExpressionStatement(this);
	}

	public class PrintStatement : Statement
    {
		public Expression Expr { get; }

		[DebuggerStepThrough]
		public PrintStatement(Expression expr)
		{
			Expr = expr;
		}

        [DebuggerStepThrough]
		public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitPrintStatement(this);
	}
}