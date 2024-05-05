namespace CraftingInterpreters.Lox
{

    public class CompilerEnvironment 
    {

        private readonly Dictionary<String,Object> values = new Dictionary<String,Object>(); 
        readonly CompilerEnvironment Enclosing;
        CompilerEnvironment Environment(){
            Enclosing = null;
        }
        CompilerEnvironment Environment(CompilerEnvironment enclosing){
            this.Enclosing = enclosing;
        }

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
            if( Enclosing != null){
                return Enclosing.Get(name);
            }
            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }

        public void Assign(Token name, Object value) {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }
            if( Enclosing != null){
                return Enclosing.Assign(name, value);
            }
            throw new RuntimeError(name,"Undefined variable '" + name.Lexeme + "'.");
        }

    
    }   
         
}