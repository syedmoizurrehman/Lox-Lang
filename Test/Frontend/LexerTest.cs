using ErrorLogger;
using Frontend;
using Frontend.Lexer;
using System.Collections.Generic;
using Xunit;
using static Frontend.TokenType;

namespace Test.Frontend
{
    public class LexerTest
    {
        [Theory]
        [InlineData(LEFT_PAREN, "(")]
        [InlineData(RIGHT_PAREN, ")")]
        [InlineData(LEFT_BRACE, "{")]
        [InlineData(RIGHT_BRACE, "}")]
        [InlineData(BANG, "!")]
        [InlineData(DOT, ".")]
        [InlineData(COMMA, ",")]
        [InlineData(SEMICOLON, ";")]
        [InlineData(STAR, "*")]
        [InlineData(SLASH, "/")]
        [InlineData(PLUS, "+")]
        [InlineData(MINUS, "-")]
        [InlineData(EQUAL, "=")]
        [InlineData(EQUAL_EQUAL, "==")]
        [InlineData(LESS, "<")]
        [InlineData(LESS_EQUAL, "<=")]
        [InlineData(GREATER, ">")]
        [InlineData(GREATER_EQUAL, ">=")]
        [InlineData(AND, "and")]
        [InlineData(CLASS, "class")]
        [InlineData(ELSE, "else")]
        [InlineData(FALSE, "false")]
        [InlineData(FOR, "for")]
        [InlineData(FUN, "fun")]
        [InlineData(IF, "if")]
        [InlineData(NIL, "nil")]
        [InlineData(OR, "or")]
        [InlineData(PRINT, "print")]
        [InlineData(RETURN, "return")]
        [InlineData(SUPER, "super")]
        [InlineData(THIS, "this")]
        [InlineData(TRUE, "true")]
        [InlineData(VAR, "var")]
        [InlineData(WHILE, "while")]
        [InlineData(IDENTIFIER, "someidentifier")]
        [InlineData(NUMBER, "123")]
        [InlineData(NUMBER, "123.2")]
        [InlineData(STRING, "\"Hello, World!\"")]
        public void IndividualTokenTypeTest(TokenType expectedType, string userInput)
        {
            var TestObj = GetTestLexer(userInput);
            IEnumerable<Token> Actual = TestObj.Tokenize();
            IEnumerator<Token> ActualEnumerator = Actual.GetEnumerator();

            Token T = default;
            if (ActualEnumerator.MoveNext())
                T = ActualEnumerator.Current;

            Assert.True(T.Type == expectedType);
        }

        [Theory]
        [InlineData("123.2.2")]
        [InlineData("123.;")]
        [InlineData("ï")]
        [InlineData("\"unterminated string")]
        public void ErrorGenerationTest(string userInput)
        {
            ErrorLoggingService.Errors.Clear();
            GetTestLexer(userInput).Tokenize();
            Assert.NotEmpty(ErrorLoggingService.Errors);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("//something\n//this")]
        public void WhitespaceAndCommentTest(string userInput)
        {
            var TestObj = GetTestLexer(userInput);
            IEnumerable<Token> Actual = TestObj.Tokenize();
            IEnumerator<Token> ActualEnumerator = Actual.GetEnumerator();
            Assert.False(ActualEnumerator.MoveNext());
        }

        private Lexer GetTestLexer(string source) => new Lexer(source);
    }
}
