namespace CraftingInterpreters.Lox
{
    public abstract class Expr
    {
        public class Binary : Expr
        {

            public Expr Left { get; }
            public Token Operator { get; }
            public Expr Right { get; }
            
            public Binary(Expr left, Token opperation, Expr right)
            {
                this.Left = left;
                this.Operator = opperation;
                this.Right = right;
            }


        }

        
    }
}