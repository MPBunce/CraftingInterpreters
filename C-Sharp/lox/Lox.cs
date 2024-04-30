using System.Runtime.CompilerServices;

namespace CraftingInterpreters.Lox
{
    public class Lox
    {
        private static readonly Interpreter interpreter = new Interpreter();
        static bool hadError = false;
        static bool hadRuntimeError = false;
        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: C#-lox [script]");
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
            if (hadError){
                Environment.Exit(65);
            }
            if (hadRuntimeError){
                Environment.Exit(70);
            } 
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
            Console.WriteLine("Running: \n");
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            Parser parser = new Parser(tokens);
            Expr expression = parser.parse();

            // Stop if there was a syntax error.
            if (hadError) return;

            //Console.WriteLine( new AstPrinter().print(expression) );
            interpreter.interpret(expression);
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        public static void Error(Token token, string message)
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

        public static void RuntimeError(RuntimeError error){
            Console.Error.WriteLine(error.Message + $"\n[line {error.token.Line}]");
            hadRuntimeError = true;
        } 
    }
}
