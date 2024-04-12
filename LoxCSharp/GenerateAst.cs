namespace CraftingInterpreters.Lox
{
    public class GenerateAst {
        public static void main(String[] args) {
            if(args.Length != 1){
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            String outputDir = args[0];

        }
    }


}