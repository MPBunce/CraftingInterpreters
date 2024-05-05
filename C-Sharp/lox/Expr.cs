namespace CraftingInterpreters.Lox {

  public abstract class Expr {


    public interface IVisitor<R> {
      R VisitAssignExpr(Assign expr);
      R VisitBinaryExpr(Binary expr);
      R VisitGroupingExpr(Grouping expr);
      R VisitLiteralExpr(Literal expr);
      R VisitUnaryExpr(Unary expr);
      R VisitVariableExpr(Variable expr);
    }

    public abstract R Accept<R>(IVisitor<R> visitor);



    public class Assign : Expr {
      public Assign(Token name, Expr value) {
        this.name = name;
        this.value = value;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
         return visitor.VisitAssignExpr(this);
      }

      public readonly Token name;
      public readonly Expr value;

    }


    public class Binary : Expr {
      public Binary(Expr Left, Token Operation, Expr Right) {
        this.Left = Left;
        this.Operation = Operation;
        this.Right = Right;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
         return visitor.VisitBinaryExpr(this);
      }

      public readonly Expr Left;
      public readonly Token Operation;
      public readonly Expr Right;

    }


    public class Grouping : Expr {
      public Grouping(Expr Expression) {
        this.Expression = Expression;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
         return visitor.VisitGroupingExpr(this);
      }

      public readonly Expr Expression;

    }


    public class Literal : Expr {
      public Literal(object Value) {
        this.Value = Value;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
         return visitor.VisitLiteralExpr(this);
      }

      public readonly object Value;

    }


    public class Unary : Expr {
      public Unary(Token Operation, Expr Right) {
        this.Operation = Operation;
        this.Right = Right;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
         return visitor.VisitUnaryExpr(this);
      }

      public readonly Token Operation;
      public readonly Expr Right;

    }


    public class Variable : Expr {
      public Variable(Token name) {
        this.name = name;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
         return visitor.VisitVariableExpr(this);
      }

      public readonly Token name;

    }

  }

}
