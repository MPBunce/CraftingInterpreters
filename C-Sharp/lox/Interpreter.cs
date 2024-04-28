namespace CraftingInterpreters.Lox {

    class Interpreter : Expr.IVisitor<Object> {

        public Object VisitLiteralExpr(Expr.Literal expr) 
        {
            return expr.Value;
        }
        public Object VisitGroupingExpr(Expr.Grouping expr) 
        {
            return Evaluate(expr.Expression);
        }

        public Object VisitUnaryExpr(Expr.Unary expr) 
        {
            Object right = Evaluate(expr.Right);

            switch (expr.Operation.Type) {
                case TokenType.BANG:
                    return !isTruthy(right);
                case TokenType.MINUS:
                    return -(double)right;
            }

            // Unreachable.
            return null;
        }
        public Object VisitBinaryExpr(Expr.Binary expr) 
        {
            Object left = Evaluate(expr.Left);
            Object right = Evaluate(expr.Right); 

            switch (expr.Operation.Type) {
            case TokenType.MINUS:
                return (double)left - (double)right;
            
            case TokenType.SLASH:
                return (double)left / (double)right;
            case TokenType.STAR:
                return (double)left * (double)right;
            }

            // Unreachable.
            return null;
        }

        private Object Evaluate(Expr expr) 
        {
            return expr.Accept(this);
        }
        private bool isTruthy(Object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }

    }

}