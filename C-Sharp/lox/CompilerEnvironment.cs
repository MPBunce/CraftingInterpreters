namespace CraftingInterpreters.Lox
{

    public class CompilerEnvironment 
    {

        private readonly Dictionary<String,Object> values = new Dictionary<String,Object>(); 

        public void Define(string name, Object value)
        {
            values[name] = value;
        }

        public Object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }
            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }

        public void Assign(Token name, Object value) {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }
            throw new RuntimeError(name,"Undefined variable '" + name.Lexeme + "'.");
        }

    
    }   
         
}