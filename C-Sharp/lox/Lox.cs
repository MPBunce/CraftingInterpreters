using System.Runtime.CompilerServices;

namespace CraftingInterpreters.Lox
{
    public class Lox
    {
        static bool hadError = false;
        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        private static void RunFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            Run(System.Text.Encoding.UTF8.GetString(bytes));
            if (hadError) Environment.Exit(65);
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null) break;
                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {

            Console.WriteLine(source);

        }

        public static void LineError(int line, string message)
        {
            Report(line, "", message);
        }

        public static void TokenError(Token token, string message)
        {
            if (token.Type == TokenType.EOF) {
                Report(token.Line, " at end", message);
            } else {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        private static void Report(int line, string where, string message) {
            Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
