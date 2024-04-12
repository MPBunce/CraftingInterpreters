namespace GenerateAst
{
    public class GenerateAst {
        public static void Main(String[] args) {
            if(args.Length != 1){
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            String outputDir = args[0];
            Console.WriteLine("test" + outputDir);

        }
    }


}