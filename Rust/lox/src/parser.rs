use crate::error::LoxError;
use crate::token::*;
use crate::expr::*;
use crate::token_type::*;


pub struct Parser {
    tokens: Vec<Token>,
    current: usize
}

impl Parser {
    pub fn new(input: Vec<Token>) -> Parser {
           Parser{
               tokens: input,
               current: 0
           }
    }

    fn expression(&mut self) -> Result<Expr, LoxError> {
        self.equality()
    }

    fn equality(&mut self) -> Result<Expr, LoxError> {
        let expr: Expr = self.comparison()?
    }

    fn comparison(&mut self) -> Result<Expr, LoxError> {

    }

    fn is_match(&mut self) -> Result<Expr, LoxError> {

    }

    fn check(&mut self, token_type: TokenType) -> bool {

    }



}