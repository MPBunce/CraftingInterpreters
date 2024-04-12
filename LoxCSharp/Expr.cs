namespace CraftingInterpreters.Lox
{
    public abstract class Expr
    {
        public class Binary : Expr
        {
            public Binary(Expr left, Token @operator, Expr right)
            {
                this.Left = left;
                this.Operator = @operator;
                this.Right = right;
            }

            public Expr Left { get; }
            public Token Operator { get; }
            public Expr Right { get; }
        }

        
    }
}