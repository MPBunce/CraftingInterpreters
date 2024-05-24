use std::fmt;
use crate::token_type::*;

#[derive(Debug, Clone, )]
pub struct Token {
    pub token_type: TokenType,
    pub lexeme: String,
    pub literal: Option<Object>,
    pub line: usize
}

#[derive(Debug, Clone)]
pub enum Object {
    Num(f64),
    Str(String),
    Nil,
    True,
    False
}

impl fmt::Display for Object {
    fn fmt(&self, f: &mut fmt::Formatter ) -> fmt::Result{
        match self {
            Object::Num(x) => write!(f, "{x}"),
            Object::Str(x) => write!(f, "{x}"),
            Object::Nil => write!(f, "nil"),
            Object::True => write!(f, "True"),
            Object::False => write!(f, "False")
        }
    }

}

impl Token {
    pub fn new( token_type: TokenType, lexeme: String,  literal: Option<Object>, line: usize) -> Token {
        Token {token_type, lexeme, literal, line}
    }

    pub fn eof(line: usize) -> Token {
        Token{
            token_type: TokenType::Eof,
            lexeme: "".to_string(),
            literal: None,
            line
        }
    }
}

impl fmt::Display for Token {
    fn fmt(&self, f: &mut fmt::Formatter ) -> fmt::Result{
        write!(f, "{:?} {} {} \n",
            self.token_type, self.lexeme,
            if let Some(literal) = &self.literal {
                literal.to_string()
            } else {
                "None".to_string()
            }
        )
    }

}