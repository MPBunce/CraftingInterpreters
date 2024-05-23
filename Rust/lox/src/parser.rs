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
        let mut expr: Expr = self.comparison()?;
        while self.is_match( &[TokenType::BangEqual, TokenType::Equal] ){
            let operator = self.previous();
            let right = self.comparison().unwrap();
            expr = Expr::Binary( BinaryExpr {
                left: Box::new(expr),
                operator,
                right: Box::new(right)
            })
        }
        Ok(expr)
    }

    fn comparison(&mut self) -> Result<Expr, LoxError> {
        let expr = self.term();

    }

    fn is_match(&mut self, token_types: &[TokenType] ) ->  bool  {
        for t in &token_types {
            if self.check(t){
                self.advance();
                return true
            }
        }

        return false;
    }

    fn check(&self, token_type: TokenType) -> bool {
        if self.is_at_end() {
            false
        } else {
            return self.peek().token_type == token_type
        }

    }

    fn advance(&mut self) -> Token {
        if self.is_at_end() {
            self.current += 1;
        }
        return self.previous()
    }

    fn is_at_end(&self) -> bool {
        self.peek().token_type == TokenType::Eof
    }

    fn peek(&self) -> Token {
        return *self.tokens.get( &self.current ).unwrap()
    }

    fn previous(&self) -> Token {
        let prev = &self.current - 1;
        return *self.tokens.get( &prev ).unwrap()
    }


}