use crate::error::LoxError;
use crate::expr::*;
use crate::token::*;
use crate::token_type::TokenType;

pub struct Interpreter;

impl ExprVisitor<Object> for Interpreter {
    fn visit_binary_expr(&self, expr: &BinaryExpr) -> Result<Object, LoxError> {

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
                return Ok(-right)
            }
            TokenType::Bang => {
                return !self.is_truthy(right)
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

    pub fn is_truthy(&self, obj: Object) -> Result<Object, LoxError>{

    }
}