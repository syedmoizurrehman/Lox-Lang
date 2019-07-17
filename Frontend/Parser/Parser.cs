using System;
using System.Collections.Generic;
using Frontend.Expressions;
using static Frontend.TokenType;
using System.Diagnostics;
using ErrorLogger;
using System.Runtime.CompilerServices;

namespace Frontend.Parser
{
    public class Parser
    {
        private IList<Token> Tokens;
        private int CurrentIndex;
        private Token PreviousToken => Tokens[CurrentIndex - 1];
        private Token CurrentToken => Tokens[CurrentIndex];
        private Token NextToken => !IsSourceConsumed() ? Tokens[CurrentIndex + 1] : default;

        public Parser(IList<Token> sourceTokens)
        {
            Tokens = sourceTokens;
            CurrentIndex = 0;
        }

        public Expression Parse()
        {
            try { return Expression(); }
            catch (Exception) { return null; }
        }

        private Expression Expression() => Equality();

        private Expression Equality()
        {
            Expression E = Comparison();
            while (MatchAny(BANG_EQUAL, EQUAL_EQUAL))
            {
                Token Operator = CurrentToken;
                AdvanceToken();
                Expression Right = Comparison();
                E = new BinaryExpression(E, Operator, Right);
            }
            return E;
        }

        private Expression Comparison()
        {
            Expression E = Addition();
            while (MatchAny(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
            {
                Token Operator = CurrentToken;
                AdvanceToken();
                Expression Right = Addition();
                E = new BinaryExpression(E, Operator, Right);
            }
            return E;
        }

        private Expression Addition()
        {
            Expression E = Multiplication();
            while (MatchAny(MINUS, PLUS))
            {
                Token Operator = CurrentToken;
                AdvanceToken();
                Expression Right = Multiplication();
                E = new BinaryExpression(E, Operator, Right);
            }
            return E;
        }

        private Expression Multiplication()
        {
            Expression E = Unary();
            while (MatchAny(SLASH, STAR))
            {
                Token Operator = CurrentToken;
                AdvanceToken();
                Expression Right = Unary();
                E = new BinaryExpression(E, Operator, Right);
            }
            return E;
        }

        private Expression Unary()
        {
            if (MatchAny(BANG, MINUS))
            {
                var Operator = CurrentToken;
                AdvanceToken();
                Expression Right = Unary();
                return new UnaryExpression(Operator, Right);
            }
            return Primary();
        }

        private Expression Primary()
        {
            switch (CurrentToken.Type)
            {
                case NUMBER:
                case STRING:
                    AdvanceToken();
                    return new LiteralExpression(PreviousToken.Literal);

                case TRUE: AdvanceToken(); return new LiteralExpression(true);
                case FALSE: AdvanceToken(); return new LiteralExpression(false);
                case NIL: AdvanceToken(); return new LiteralExpression(null);

                case LEFT_PAREN:
                    AdvanceToken();
                    Expression E = Expression();
                    if (CurrentToken.Type == RIGHT_PAREN)
                    {
                        AdvanceToken();
                        return new GroupingExpression(E);
                    }
                    AddError("Expected ')'.");
                    throw ReportError();     // Error
            }
            throw ReportError();
        }

        private Exception ReportError()
        {
            return new Exception();
        }

        private void Synchronize()
        {
            AdvanceToken();
            while (!IsSourceConsumed())
            {
                if (PreviousToken.Type == SEMICOLON) return;

                switch (NextToken.Type)
                {
                    case CLASS:
                    case FUN:
                    case VAR:
                    case FOR:
                    case IF:
                    case WHILE:
                    case PRINT:
                    case RETURN:
                        return;
                }

                AdvanceToken();
            }
        }

        private void AddError(string message)
        {
            string ErrorMessage;
            if (CurrentToken.Type == EOF)
                ErrorMessage = $"At end of file: {message}";
            else
                ErrorMessage = $"On {CurrentToken.Lexeme}: {message}";

            ErrorLoggingService.Errors.Add(new Error(ErrorMessage, CurrentToken.LineNumber, CurrentToken.ColumnNumber));
        }

        [DebuggerStepThrough]
        private bool MatchAny(params TokenType[] tokenTypes)
        {
            if (!IsSourceConsumed())
            {
                foreach (var T in tokenTypes)
                    if (T == CurrentToken.Type) return true;
            }

            return false;
        }

        [DebuggerStepThrough]
        private void AdvanceToken() => CurrentIndex++;

        [DebuggerStepThrough]
        private bool IsSourceConsumed() => CurrentIndex >= Tokens.Count;
    }
}