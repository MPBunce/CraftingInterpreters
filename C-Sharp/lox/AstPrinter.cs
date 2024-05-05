using System.Text;

namespace CraftingInterpreters.Lox {

    public class AstPrinter : Expr.IVisitor<string>  {

        //Used to test the AstPrinter
        // public static void Main(String[] args) {
        //     Expr expression = new Expr.Binary(
        //         new Expr.Unary(
        //             new Token(TokenType.MINUS, "-", null, 1),
        //             new Expr.Literal(123)),
        //         new Token(TokenType.STAR, "*", null, 1),
        //         new Expr.Grouping(
        //             new Expr.Literal(45.67)));

        //     Console.WriteLine(new AstPrinter().print(expression));
        // }

        public string print(Expr expr ){
            return expr.Accept(this);
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.Operation.Lexeme, expr.Left, expr.Right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return Parenthesize("group", expr.Expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            if (expr.Value == null) return "nil";
            return expr.Value.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return Parenthesize(expr.Operation.Lexeme, expr.Right);
        }

        private string Parenthesize(string name, params Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }

        public string VisitAssignExpr(Expr.Assign assign){
            return "null";
        }

        public string VisitVariableExpr(Expr.Variable variable){
            return "null";
        }

    }

}