using ErrorLogger;
using Frontend.Expressions;
using Frontend.Lexer;
using Frontend.Parser;
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
                var Tokens = LexicalAnalyzer.Tokenize();
                //foreach (var Token in Tokens)
                //Console.WriteLine(Token);
                if (ErrorLoggingService.Errors.Count == 0)
                {
                    var P = new Parser(Tokens);
                    var Program = P.Parse();
                    if (Program != null)
                    {
                        foreach (var Statement in Program)
                        {
                            Expression E = null;
                            switch (Statement)
                            {
                                case Frontend.Statements.ExpressionStatement ExprStmt:
                                    E = ExprStmt.Expr; break;

                                case Frontend.Statements.PrintStatement PntStmt:
                                    E = PntStmt.Expr; break;
                            }
                            Console.WriteLine(new AstPrinter().Print(E));
                        }
                    }
                    else
                        Console.WriteLine("Syntax error(s) in expression tree.");
                }

                foreach (Error E in ErrorLoggingService.Errors)
                    Console.WriteLine(E);

                ErrorLoggingService.Errors.Clear();
            }
        }
    }
}
