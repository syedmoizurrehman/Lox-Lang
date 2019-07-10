using ErrorLogger;
using System;
using System.Collections.Generic;
using static Frontend.Lexer.TokenType;

namespace Frontend.Lexer
{
    public class Lexer
    {
        readonly List<Token> Tokens;
        int CurrentLine;
        readonly string Source;
        int StartIndex;
        int CurrentIndex;
        LexerState CurrentState;

        public Lexer(string source)
        {
            Source = source;
            Tokens = new List<Token>();
            CurrentLine = 0;
            StartIndex = CurrentIndex = -1;
            CurrentState = LexerState.Undetermined;
        }

        enum LexerState
        {
            Undetermined, StringLit, NumberLit, Comment, Identifier
        }

        public IEnumerable<Token> Tokenize()
        {
            bool IsFraction = false;
            while (!IsSourceConsumed())
            {
                CurrentIndex++;
                char C = Source[CurrentIndex];
                switch (CurrentState)
                {
                    case LexerState.Undetermined:
                        switch (C)
                        {
                            case '(': MarkSymbolStart(); AddToken(LEFT_PAREN); break;
                            case ')': MarkSymbolStart(); AddToken(RIGHT_PAREN); break;
                            case '{': MarkSymbolStart(); AddToken(LEFT_BRACE); break;
                            case '}': MarkSymbolStart(); AddToken(RIGHT_BRACE); break;
                            case ',': MarkSymbolStart(); AddToken(COMMA); break;
                            case '.': MarkSymbolStart(); AddToken(DOT); break;
                            case ';': MarkSymbolStart(); AddToken(SEMICOLON); break;
                            case '-': MarkSymbolStart(); AddToken(MINUS); break;
                            case '+': MarkSymbolStart(); AddToken(PLUS); break;
                            case '*': MarkSymbolStart(); AddToken(STAR); break;
                            case '!': MarkSymbolStart(); if (Peek() == '=') { AddToken(BANG_EQUAL); CurrentIndex++; } else AddToken(BANG); break;
                            case '=': MarkSymbolStart(); if (Peek() == '=') { AddToken(EQUAL_EQUAL); CurrentIndex++; } else AddToken(EQUAL); break;
                            case '<': MarkSymbolStart(); if (Peek() == '=') { AddToken(LESS_EQUAL); CurrentIndex++; } else AddToken(LESS); break;
                            case '>': MarkSymbolStart(); if (Peek() == '=') { AddToken(GREATER_EQUAL); CurrentIndex++; } else AddToken(GREATER); break;
                            case '/': MarkSymbolStart(); if (Peek() == '/') { CurrentState = LexerState.Comment; CurrentIndex++; } else AddToken(SLASH); break;

                            // Whitespaces.                      
                            case ' ':
                            case '\r':
                            case '\t':
                                break;

                            case '\n':
                                CurrentLine++;
                                break;

                            // Begin string literals.
                            case '"':
                                CurrentState = LexerState.StringLit;
                                MarkSymbolStart();
                                break;

                            // Misc.
                            default:
                                // Begin number literals.
                                if (IsEnglishDigit(C))
                                {
                                    CurrentState = LexerState.NumberLit;
                                    MarkSymbolStart();
                                }
                                else if (IsEnglishAlphabet(C))
                                {
                                    CurrentState = LexerState.Identifier;
                                    MarkSymbolStart();
                                }
                                else { AddError("Invalid character."); ResetState(); }
                                break;
                        }
                        break;

                    case LexerState.StringLit:
                        switch (C)
                        {
                            // End of literal.
                            case '"':
                                ResetState();
                                AddToken(STRING, GetStringLiteral());
                                break;

                            case '\n': CurrentLine++; break;    // Line break is considered part of literal.
                        }
                        break;

                    case LexerState.NumberLit:
                        switch (C)
                        {
                            case '.':
                                if (!IsFraction)
                                {
                                    if (IsEnglishDigit(Peek())) IsFraction = true;
                                    else { AddError("'.' at the end of literal."); ResetState(); }
                                }
                                else { AddError("Extra '.' in number literal. Only one '.' is allowed in number literals."); ResetState(); }
                                break;

                            default:
                                // End of literal.
                                if (!IsEnglishDigit(C))
                                {
                                    ResetState();
                                    CurrentIndex--;
                                    AddToken(NUMBER, Convert.ToDouble(GetLexemeString()));
                                }
                                break;
                        }
                        break;

                    case LexerState.Identifier:
                        // End of identifier.
                        if (!IsAlphanumeric(C))
                        {
                            ResetState();
                            CurrentIndex--;
                            TokenType Type;
                            switch (GetLexemeString())
                            {
                                case "and": Type = AND; break;
                                case "class": Type = CLASS; break;
                                case "else": Type = ELSE; break;
                                case "false": Type = FALSE; break;
                                case "for": Type = FOR; break;
                                case "fun": Type = FUN; break;
                                case "if": Type = IF; break;
                                case "nil": Type = NIL; break;
                                case "or": Type = OR; break;
                                case "print": Type = PRINT; break;
                                case "return": Type = RETURN; break;
                                case "super": Type = SUPER; break;
                                case "this": Type = THIS; break;
                                case "true": Type = TRUE; break;
                                case "var": Type = VAR; break;
                                case "while": Type = WHILE; break;
                                default: Type = IDENTIFIER; break;
                            }
                            AddToken(Type);
                        }
                        break;

                    case LexerState.Comment:
                        if (C == '\n') { CurrentLine++; ResetState(); }
                        break;
                }
            }

            if (CurrentState != LexerState.Undetermined)
            {
                switch (CurrentState)
                {
                    case LexerState.StringLit:
                        // Unexpected EOF in string literal.
                        AddError("Unterminated string literal.");
                        ResetState();
                        break;
                        
                    case LexerState.NumberLit:
                        AddToken(NUMBER, Convert.ToDouble(GetLexemeString()));
                        break;
                        
                    case LexerState.Identifier:
                        TokenType Type;
                        switch (GetLexemeString())
                        {
                            case "and": Type = AND; break;
                            case "class": Type = CLASS; break;
                            case "else": Type = ELSE; break;
                            case "false": Type = FALSE; break;
                            case "for": Type = FOR; break;
                            case "fun": Type = FUN; break;
                            case "if": Type = IF; break;
                            case "nil": Type = NIL; break;
                            case "or": Type = OR; break;
                            case "print": Type = PRINT; break;
                            case "return": Type = RETURN; break;
                            case "super": Type = SUPER; break;
                            case "this": Type = THIS; break;
                            case "true": Type = TRUE; break;
                            case "var": Type = VAR; break;
                            case "while": Type = WHILE; break;
                            default: Type = IDENTIFIER; break;
                        }
                        AddToken(Type);
                        break;
                        
                    case LexerState.Comment:
                        // Not an error.
                        break;
                }
            }
            return Tokens;
        }

        private string GetLexemeString() => Source.Substring(StartIndex, CurrentIndex - StartIndex + 1);

        private string GetStringLiteral() => Source.Substring(StartIndex + 1, CurrentIndex - StartIndex - 1);

        private bool IsAlphanumeric(char c) => IsEnglishAlphabet(c) || IsEnglishDigit(c);

        private bool IsEnglishAlphabet(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

        private void MarkSymbolStart() => StartIndex = CurrentIndex;

        private void ResetState() => CurrentState = LexerState.Undetermined;

        private void AddToken(TokenType type, object literal = null) => Tokens.Add(new Token(type, GetLexemeString(), literal, CurrentLine, StartIndex));

        private void AddError(string errorMessage) => ErrorLoggingService.Errors.Add(new Error(errorMessage, CurrentLine, CurrentIndex));

        private bool IsEnglishDigit(char c) => c >= '0' && c <= '9';

        private char Peek() => IsSourceConsumed() ? '\0' : Source[CurrentIndex + 1];

        private bool IsSourceConsumed() => CurrentIndex >= Source.Length - 1;
    }
}
