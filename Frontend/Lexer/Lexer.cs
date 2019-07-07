using System;
using System.Collections.Generic;

namespace Frontend.Lexer
{
    public class Lexer
    {
        readonly List<Token> Tokens;
        int CurrentLine = 0;
        readonly string Source;
        int StartIndex;
        int CurrentIndex;

        public Lexer(string source)
        {
            Source = source;
            Tokens = new List<Token>();
            CurrentLine = 0;
            StartIndex = CurrentIndex = -1;
        }

        public Token[] Tokenize()
        {
            throw new NotImplementedException();
        }
    }
}