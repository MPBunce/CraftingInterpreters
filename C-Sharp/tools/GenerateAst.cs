namespace GenerateAst
{
    public class GenerateAst {
        public static void Main(String[] args) {
            if(args.Length != 1){
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            String outputDir = args[0];

            DefineAst(outputDir, "Expr", new List<string>
            {
                "Binary   : Expr Left, Token Operator, Expr Right",
                "Grouping : Expr Expression",
                "Literal  : object Value",
                "Unary    : Token Operator, Expr Right"
            });
        }

        static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = Path.Combine(outputDir, baseName + ".cs");
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine();
                writer.WriteLine($"namespace CraftingInterpreters.Lox");
                writer.WriteLine("{");
                writer.WriteLine($"    public abstract class {baseName}");
                writer.WriteLine("    {");

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }

    }

}