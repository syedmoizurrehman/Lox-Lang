using Frontend.Lexer;
using System;
using System.IO;

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
            {
                RunFile(args[1]);
            }
            else
            {
                RunPrompt();
            }
        }

        static void RunFile(string filePath)
        {

        }

        static string RunPrompt()
        {
            while (true)
            {
                Console.Write(">");
                string Input = Console.ReadLine();
                var LexicalAnalyzer = new Lexer(Input);
                foreach (var Token in LexicalAnalyzer.Tokenize())
                    Console.WriteLine(Token);
            }
        }

        static void GenerateError(int lineNumber, string message)
        {
            using (TextWriter ErrorWriter = Console.Error)
            {
                ErrorWriter.WriteLine($"At line {lineNumber}, " + message);
            }
        }
    }
}