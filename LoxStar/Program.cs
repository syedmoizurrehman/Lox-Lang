using ErrorLogger;
using Frontend.Lexer;
using System;

namespace LoxStar
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: loxstar [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
                RunFile(args[1]);
            else
                RunPrompt();
        }

        static void RunFile(string filePath)
        {
            throw new NotImplementedException();
        }

        static string RunPrompt()
        {
            while (true)
            {
                Console.Write("LoxStar>");
                string Input = Console.ReadLine();
                var LexicalAnalyzer = new Lexer(Input);
                foreach (var Token in LexicalAnalyzer.Tokenize())
                    Console.WriteLine(Token);

                foreach (Error E in ErrorLoggingService.Errors)
                    Console.WriteLine($"At line {E.LineNumber}, col {E.ColumnNumber}: {E.Message}");

                ErrorLoggingService.Errors.Clear();
            }
        }
    }
}
