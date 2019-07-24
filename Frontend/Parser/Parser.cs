using System;
using System.Collections.Generic;
using Frontend.Expressions;
using static Frontend.TokenType;
using System.Diagnostics;
using ErrorLogger;

namespace Frontend.Parser
{
    [Serializable]
    public class ParseException : Exception
    {
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception inner) : base(message, inner) { }
        protected ParseException(System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class Parser
    {
        private IList<Token> Tokens;
        private int CurrentIndex;
        private Token PreviousToken => Tokens[CurrentIndex - 1];
        private Token CurrentToken => !IsSourceConsumed() ? Tokens[CurrentIndex] : new Token(EOF, null, null, PreviousToken.LineNumber, PreviousToken.ColumnNumber);
        private Token NextToken => CurrentIndex + 1 < Tokens.Count ? Tokens[CurrentIndex + 1] : new Token(EOF, null, null, PreviousToken.LineNumber, PreviousToken.ColumnNumber);

        public Parser(IList<Token> sourceTokens)
        {
            Tokens = sourceTokens;
            CurrentIndex = 0;
        }

        public Expression Parse()
        {
            try { return Expression(); }
            catch (ParseException)
            {
                if (!IsSourceConsumed())
                {
                    Synchronize();
                    return Parse();
                }
                return null;
            }
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
                    throw ReportError("Expected ')'.");     // Error
            }
            return null;
        }

        private ParseException ReportError(string message)
        {
            AddError(message);
            return new ParseException();
        }

        private void Synchronize()
        {
            AdvanceToken();
            while (!IsSourceConsumed())
            {
                if (PreviousToken.Type == SEMICOLON) return;

                switch (CurrentToken.Type)
                {
                    case STRING:
                    case NUMBER:
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
            string ErrorMessage = message;
            if (CurrentToken.Type == EOF)
                ErrorMessage = $"At end of file: {message}";

            ErrorLoggingService.Errors.Add(new Error(ErrorType.Syntax, ErrorMessage, CurrentToken.LineNumber, CurrentToken.ColumnNumber));
        }

        private void AddError(Token errorToken, string message)
        {
            string ErrorMessage = message;
            if (errorToken.Type == EOF)
                ErrorMessage = $"At end of file: {message}";

            ErrorLoggingService.Errors.Add(new Error(ErrorType.Syntax, ErrorMessage, errorToken.LineNumber, errorToken.ColumnNumber));
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