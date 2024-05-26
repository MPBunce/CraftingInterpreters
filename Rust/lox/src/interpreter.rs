use crate::error::LoxError;
use crate::expr::*;
use crate::object::*;
use crate::token_type::TokenType;

pub struct Interpreter;

impl ExprVisitor<Object> for Interpreter {
    fn visit_binary_expr(&self, expr: &BinaryExpr) -> Result<Object, LoxError> {
        let left = self.evaluate(&expr.left).unwrap();
        let right =self.evaluate(&expr.right).unwrap();
        match expr.operator.token_type {
            TokenType::Minus => {
                if let Object::Num(left) = left {
                    if let Object::Num(right) = right {
                        return  Ok( Object::Num(left - right) )
                    }
                }
                return Err(LoxError::error(expr.operator.line, "Error Binary Exp" ))
            }
            TokenType::Slash => {
                if let Object::Num(left) = left {
                    if let Object::Num(right) = right {
                        return  Ok( Object::Num(left / right) )
                    }
                }
                return Err(LoxError::error(expr.operator.line, "Error Binary Exp" ))
            }
            TokenType::Star => {
                if let Object::Num(left) = left {
                    if let Object::Num(right) = right {
                        return  Ok( Object::Num(left * right) )
                    }
                }
                return Err(LoxError::error(expr.operator.line, "Error Binary Exp" ))
            }
            TokenType::Plus => {
                if let Object::Num(left) = left {
                    if let Object::Num(right) = right {
                        return  Ok( Object::Num(left + right) )
                    }
                }
                return Err(LoxError::error(expr.operator.line, "Error Binary Exp"))
            }
            _ => {
                return Err( LoxError::error(expr.operator.line, "Error Binary Exp" ) )
            }
        }
    }

    fn visit_grouping_expr(&self, expr: &GroupingExpr) -> Result<Object, LoxError> {
        Ok( self.evaluate( &expr.expression ).unwrap() )
    }

    fn visit_literal_expr(&self, expr: &LiteralExpr) -> Result<Object, LoxError> {
        let a = expr.value.clone().unwrap();
        Ok(a)
    }

    fn visit_unary_expr(&self, expr: &UnaryExpr) -> Result<Object, LoxError> {
        let right = self.evaluate(&expr.right).unwrap();
        match expr.operator.token_type {
            TokenType::Minus => {
                if let Object::Num(right) = right {
                    return Ok( Object::Num( - right) )
                }
                return Err( LoxError::error(expr.operator.line, "Error Binary Exp" ) )
            }
            TokenType::Bang => {
                if self.is_truthy(right) {
                    Ok(Object::Bool(false))
                } else {
                    Ok(Object::Bool(true))
                }
            }
            _ => {
                return Ok(Object::Nil)
            }
        }
    }


}

impl Interpreter {
    pub fn evaluate(&self, expr: &Expr) -> Result<Object, LoxError> {
            expr.accept(self)
    }

    pub fn is_truthy(&self, obj: Object) -> bool {
        !matches!(obj, Object::Nil | Object::Bool(false))
    }
}