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
                "Binary   : Expr Left, Token Operation, Expr Right",
                "Grouping : Expr Expression",
                "Literal  : object Value",
                "Unary    : Token Operation, Expr Right"
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

                defineVisitor(writer, baseName, types);

                foreach (string type in types)
                {
                    string[] parts = type.Split(':');
                    string className = parts[0].Trim();
                    string fields = parts[1].Trim();
                    defineType(writer, baseName, className, fields);
                }



                writer.WriteLine();
                writer.WriteLine("    public abstract R Accept<R>(Visitor<R> visitor);");


                writer.WriteLine("    }");
                writer.WriteLine("}");

                writer.Close();
            }

        }

        private static void defineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine("public interface Visitor<R> {");

            foreach (string type in types)
            {
                String typeName = type.Split(":")[0].Trim();
                writer.WriteLine("    R Visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
            }

            writer.WriteLine("  }");
        }

        private static void defineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine($"       public class {className} : {baseName}");
            writer.WriteLine("       {");

            // Constructor.
            writer.WriteLine($"         public {className}({fieldList})");
            writer.WriteLine("         {");

            // Store parameters in fields.
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(' ')[1];
                writer.WriteLine($"           this.{name} = {name};");
            }

            writer.WriteLine("          }");

            writer.WriteLine();
            writer.WriteLine("    public override R Accept<R>(Visitor<R> visitor)");
            writer.WriteLine("    {");
            writer.WriteLine($"        return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("    }");



            // Fields.
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine($"         public readonly {field};");
            }

            writer.WriteLine("        }");
        }



    }

}