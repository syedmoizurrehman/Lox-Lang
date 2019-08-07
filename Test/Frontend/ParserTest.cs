using Frontend;
using Frontend.Parser;
using Frontend.Lexer;
using Frontend.Expressions;
using System.Collections.Generic;
using Xunit;
using System;
using Frontend.Statements;

namespace Test.Frontend
{
    public class ParserTest
    {
        [Fact]
        public void BinExprTest()
        {
            var P = GetTestParser("123 * 456");
            var S = (ExpressionStatement)P.Parse()[0];
            var E = (BinaryExpression)S.Expr;
            var L = (LiteralExpression)E.Left;
            var Op = E.BinaryOperator;
            var R = (LiteralExpression)E.Right;
            Assert.True((double)L.Value == 123 && (double)R.Value == 456 && Op.Type == TokenType.STAR);
        }

        [Fact]
        public void UnExprTest()
        {
            var P = GetTestParser("-123");
            var S = (ExpressionStatement)P.Parse()[0];
            var E = (UnaryExpression)S.Expr;
            var Op = E.UnaryOperator;
            var R = (LiteralExpression)E.Right;
            Assert.True(Op.Type == TokenType.MINUS && (double)R.Value == 123);
        }

        [Theory]
        [InlineData(typeof(BinaryExpression), "123 * 456")]
        [InlineData(typeof(BinaryExpression), "123 != 456")]
        [InlineData(typeof(GroupingExpression), "(1)")]
        [InlineData(typeof(LiteralExpression), "1")]
        [InlineData(typeof(LiteralExpression), "\"This\"")]
        [InlineData(typeof(LiteralExpression), "true")]
        [InlineData(typeof(UnaryExpression), "-1")]
        public void ExprTypeTest(Type expectedType, string userInput)
        {
            var P = GetTestParser(userInput);
            var S = (ExpressionStatement)P.Parse()[0];
            var Root = S.Expr;
            Assert.True(Root.GetType() == expectedType);
        }

        [Theory]
        [InlineData("+", "123 + 456")]
        [InlineData("-", "123 - 456")]
        [InlineData("*", "123 * 456")]
        [InlineData("/", "123 / 456")]
        [InlineData(">", "123 > 456")]
        [InlineData("<", "123 < 456")]
        [InlineData("<=", "123 <= 456")]
        [InlineData(">=", "123 >= 456")]
        [InlineData("==", "123 == 456")]
        [InlineData("!=", "123 != 456")]
        public void BinExprOperatorTest(string expectedOperator, string userInput)
        {
            var P = GetTestParser(userInput);
            var S = (ExpressionStatement)P.Parse()[0];
            var Root = S.Expr as BinaryExpression;
            Assert.True(Root.BinaryOperator.Lexeme == expectedOperator);
        }

        [Theory]
        [InlineData("!", "!true")]
        [InlineData("-", "-1")]
        public void UnaryExprOperatorTest(string expectedOperator, string userInput)
        {
            var P = GetTestParser(userInput);
            var S = (ExpressionStatement)P.Parse()[0];
            var Root = S.Expr as UnaryExpression;
            Assert.True(Root.UnaryOperator.Lexeme == expectedOperator);
        }

        private Parser GetTestParser(string source) => new Parser(GetTokens(source));

        private IList<Token> GetTokens(string source) => new Lexer(source).Tokenize();
    }
}
