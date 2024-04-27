using System.Runtime.CompilerServices;

namespace GenerateAst
{
    public class GenerateAst {

        public static void Main(String[] args) {
            if(args.Length != 1){
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            
            String OutputDir = args[0];
            List<string> MyList = new List<string>
            {
                "Binary   : Expr Left, Token Operation, Expr Right",
                "Grouping : Expr Expression",
                "Literal  : object Value",
                "Unary    : Token Operation, Expr Right"
            };


            DefineAst(OutputDir, "Expr", MyList);
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types){

            string path = Path.Combine(outputDir, baseName + ".cs");

            using (StreamWriter writer = new StreamWriter(path)) {
                writer.WriteLine("namespace CraftingInterpreters.Lox {");
                writer.WriteLine();
                writer.WriteLine($"  public class {baseName} {{");
                writer.WriteLine();
                writer.WriteLine("  }");
                writer.WriteLine();
                writer.WriteLine("}");
            }

        }


    }
}