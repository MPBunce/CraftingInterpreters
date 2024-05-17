use crate::token::*;
use crate::error::*;
use crate::token_type::TokenType;

pub struct Scanner {
    source: Vec<char>,
    tokens: Vec<Token>,
    start: usize,
    current: usize,
    line: usize
}

impl Scanner {
    pub fn new(source: String) -> Scanner{
        Scanner {
            source: source.chars().collect(),
            tokens: Vec::new(),
            start: 0,
            current: 0,
            line: 1
        }
    }
    pub fn scan_tokens(&mut self) -> Result<&Vec<Token>, LoxError>{
        while !self.is_at_end(){
            self.start = self.current;
            match self.scan_token(){
                Ok(_) => {}
                Err(e) => {
                    e.report("".to_string());
                }
            }
        }

        self.tokens.push( Token::eof(self.line) );

        Ok(&self.tokens)
    }
    fn scan_token(&mut self) -> Result<(), LoxError>{
        let c = self.advance();
        match c {
            '(' => self.add_token(TokenType::LeftParen),
            ')' => self.add_token(TokenType::RightParen),
            '{' => self.add_token(TokenType::LeftBrace),
            '}' => self.add_token(TokenType::RightBrace),
            ',' => self.add_token(TokenType::Comma),
            '.' => self.add_token(TokenType::Dot),
            '-' => self.add_token(TokenType::Minus),
            '+' => self.add_token(TokenType::Plus),
            ';' => self.add_token(TokenType::Semicolon),
            '*' => self.add_token(TokenType::Star),
            _ => {
                return Err(
                    LoxError::error(self.line, "Unexpected Char".to_string() )
                )
            }

        }
        Ok(())
    }
    fn is_at_end(&self) -> bool{
        self.current >= self.source.len()
    }

    fn advance(&mut self) -> char{
        let res = *self.source.get(self.current).unwrap();
        self.current += 1;
        return res;
    }
    fn add_token(&mut self, token_type: TokenType){
        self.add_token_object(token_type, None);
    }
    fn add_token_object(&mut self, token_type: TokenType, literal: Option<Object>){
        let lexeme: String = self.source[self.start..self.current].iter().collect();
        self.tokens.push( Token::new(token_type, lexeme, literal, self.line) );
    }

}