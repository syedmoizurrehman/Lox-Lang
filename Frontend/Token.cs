namespace Frontend
{
    public struct Token
    {
        public TokenType Type { get; }

        public string Lexeme { get; }

        public object Literal { get; }

        public int LineNumber { get; }

        public int ColumnNumber { get; }

        public Token(TokenType type, string lexeme, object literal, int lineNumber, int columnNumber)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public override string ToString() => Type + " " + Lexeme + " " + Literal;
    }

    public enum TokenType
    {
        // Single-character tokens.                      
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

        // One or two character tokens.                  
        QUESTION, COLON,
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals.                                     
        IDENTIFIER, STRING, NUMBER,

        // Keywords.                                     
        AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

        EOF
    }
}
