use crate::token::*;

#[derive(Debug)]
pub struct LoxError {
    line: usize,
    message: String
}
impl LoxError {
    pub fn error( line: usize, message: String) -> LoxError{
        LoxError {line, message}
    }
    pub fn token_error( t: Token, message: String) -> LoxError{
        LoxError::error(t.line, message)
    }

    pub fn report(&self, loc: String ){
        eprintln!("[Line {}] Error{}: {}", self.line, loc, self.message);
    }
    pub fn parser_report(&self){
        println!("Error");
    }

}
