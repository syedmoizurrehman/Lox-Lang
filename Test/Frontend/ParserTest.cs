using Frontend;
using Frontend.Parser;
using Frontend.Lexer;
using Frontend.Expressions;
using System.Collections.Generic;
using Xunit;
using System;

namespace Test.Frontend
{
    public class ParserTest
    {
        [Theory]
        [InlineData(typeof(BinaryExpression), "123 , 456")]
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
            var Root = P.Parse();
            Assert.True(Root.GetType() == expectedType);
        }

        [Theory]
        [InlineData(",", "123 , 456")]
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
            var Root = P.Parse() as BinaryExpression;
            Assert.True(Root.BinaryOperator.Lexeme == expectedOperator);
        }

        [Theory]
        [InlineData("!", "!true")]
        [InlineData("-", "-1")]
        public void UnaryExprOperatorTest(string expectedOperator, string userInput)
        {
            var P = GetTestParser(userInput);
            var Root = P.Parse() as UnaryExpression;
            Assert.True(Root.UnaryOperator.Lexeme == expectedOperator);
        }

        private Parser GetTestParser(string source) => new Parser(GetTokens(source));

        private IList<Token> GetTokens(string source) => new Lexer(source).Tokenize();
    }
}
