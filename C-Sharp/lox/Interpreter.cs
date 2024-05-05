namespace CraftingInterpreters.Lox {

    class Interpreter : Expr.IVisitor<Object>, Stmt.IVisitor<Object> {

        private CompilerEnvironment environment = new CompilerEnvironment();

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
                    CheckNumberOperand(expr.Operation, right);
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
                case TokenType.BANG_EQUAL: 
                    return !isEqual(left, right);
                case TokenType.EQUAL_EQUAL: 
                    return isEqual(left, right);
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if( left is Double && right is Double){
                        return (double)left + (double)right;
                    }
                    if( left is String && right is String){
                        return (string)left + (string)right;
                    }
                    break;
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operation, left, right);
                    return (double)left * (double)right;
            }

            // Unreachable.
            return null;
        }

        private Object Evaluate(Expr expr) 
        {
            return expr.Accept(this);
        }

        public Object VisitExpressionStmt(Stmt.Expression stmt){
            Evaluate(stmt.expression);
            return null;
        }

        public Object VisitPrintStmt(Stmt.Print stmt){
            Object value = Evaluate(stmt.expression);
            Console.WriteLine( stringify(value) );
            return null;
        }

        public Object VisitVarStmt(Stmt.Var stmt){
            Object value = null;
            if(stmt.initializer != null){
                value = Evaluate(stmt.initializer);
            }
            environment.Define(stmt.name.Lexeme, value);
            return null;
        }

        public Object VisitVariableExpr( Expr.Variable expr){
            return environment.Get(expr.name);
        }
        
        public Object VisitAssignExpr(Expr.Assign expr){
            Object value = Evaluate(expr.value);
            environment.Assign(expr.name, value);
            return value;
        }

        public Object VisitBlockStmt(){
            ExecuteBlock(stmt.statements, new Environment(environment));
            return null;
        }

        public void interpret(List<Stmt> statements){
            try {
                foreach(Stmt statement in statements){
                    //Console.WriteLine("statement" + statement);
                    Execute(statement);
                } 
            } catch (RuntimeError error){
                Lox.RuntimeError(error);
            }
        }
        
        private void Execute(Stmt stmt){
            stmt.Accept(this);
        }

        private void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            CompilerEnvironment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }


        private bool isTruthy(Object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }

        private bool isEqual(Object a, Object b)
        {
            if (a == null && b == null){
                return true;
            }
            if(a == null){
                return false;
            }
            return a.Equals(b);
        }

        private string stringify(Object obj){
            if( obj == null){
                return "nil";
            }
            if(obj is Double){
                string text = obj.ToString();
                if( text.EndsWith(".0") ){
                    text = text[0..(text.Length - 2)];
                }
                return text;
            }
            return obj.ToString();
        }

        private void CheckNumberOperand(Token operation, Object operand){
            if(operand is Double){
                return;
            }
            throw new RuntimeError(operation, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token operation, Object left, Object right){
            if(left is Double && right is Double){
                return;
            }
            throw new RuntimeError(operation, "Operands must be numbers.");
        }

    }

}