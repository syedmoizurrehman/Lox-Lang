using System;
using System.Collections.Generic;
using System.IO;

namespace ErrorLogger
{
    public static class ErrorLoggingService
    {
        public static IList<Error> Errors { get; } = new List<Error>();

        public static void GenerateError(int lineNumber, string message)
        {
            using (TextWriter ErrorWriter = Console.Error)
            {
                ErrorWriter.WriteLine($"At line {lineNumber}, " + message);
            }
        }

        public static void DisplayError(Error error)
        {
            using (TextWriter ErrorWriter = Console.Error)
            {
                ErrorWriter.WriteLine(error);
            }
        }
    }

    public enum ErrorType
    {
        Misc, Lexical, Syntax
    }

    public struct Error
    {
        public string Message { get; }

        public int LineNumber { get; }

        public int ColumnNumber { get; }

        public ErrorType Type { get; set; }

        public Error(ErrorType type, string message, int line, int column)
        {
            Type = type;
            Message = message;
            LineNumber = line;
            ColumnNumber = column;
        }

        public override string ToString()
        {
            var Error = "";
            switch (Type)
            {
                case ErrorType.Misc: Error = "Error:"; break;
                case ErrorType.Lexical: Error = "Lexical Error:"; break;
                case ErrorType.Syntax: Error = "Syntax Error:"; break;
            }
            return Error + $" At line {LineNumber}, col {ColumnNumber}: " + Message;
        }
    }
}
