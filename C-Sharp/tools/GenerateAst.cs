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
                "Assign   : Token name, Expr value",
                "Binary   : Expr Left, Token Operation, Expr Right",
                "Grouping : Expr Expression",
                "Literal  : object Value",
                "Logical  : Expr left, Token Operation, Expr right",
                "Unary    : Token Operation, Expr Right",
                "Variable : Token name"
            };
            DefineAst(OutputDir, "Expr", MyList);


            List<string> StmtList = new List<string>
            {
                "Block      : List<Stmt> statements",
                "Expression : Expr expression",
                "If         : Expr condition, Stmt thenBranch, Stmt elseBranch",
                "Print      : Expr expression",
                "Var        : Token name, Expr initializer",
                "While      : Expr condition, Stmt body"
            };


            DefineAst(OutputDir, "Stmt", StmtList);
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types){

            string path = Path.Combine(outputDir, baseName + ".cs");

            using (StreamWriter writer = new StreamWriter(path)) {
                writer.WriteLine("namespace CraftingInterpreters.Lox {");
                writer.WriteLine();
                writer.WriteLine($"  public abstract class {baseName} {{");

                writer.WriteLine();
                writer.WriteLine();
                DefineVisitor(writer, baseName, types);
                writer.WriteLine();
                writer.WriteLine("    public abstract R Accept<R>(IVisitor<R> visitor);");
                writer.WriteLine();

                foreach (string type in types){
                    string[] parts = type.Split(':');
                    string ClassName = parts[0].Trim();
                    string fields = parts[1].Trim();
                    DefineType(writer, baseName, ClassName, fields);
                }

                writer.WriteLine();
                writer.WriteLine("  }");
                writer.WriteLine();
                writer.WriteLine("}");
            }

        }
        
        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types){
            writer.WriteLine("    public interface IVisitor<R> {");

            foreach (string type in types)
            {
                string[] parts = type.Split(':');
                string typeName = parts[0].Trim();
                writer.WriteLine($"      R Visit{typeName}{baseName}(" + $"{typeName} {baseName.ToLower()});");
            }

            writer.WriteLine("    }");
        }



        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList){
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine($"    public class {className} : {baseName} {{");

            // Constructor.
            writer.WriteLine($"      public {className}({fieldList}) {{");

            // Store parameters in fields.
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(' ')[1];
                writer.WriteLine($"        this.{name} = {name};");
            }

            writer.WriteLine("      }");

            //Visitor Pattern
            writer.WriteLine();
            writer.WriteLine("      public override R Accept<R>(IVisitor<R> visitor) {");
            writer.WriteLine($"         return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("      }");


            // Fields.
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine($"      public readonly {field};");
            }
            writer.WriteLine();
            writer.WriteLine("    }");

        }

    }
}