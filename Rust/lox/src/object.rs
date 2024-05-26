use std::fmt;
use std::ops::*;

#[derive(Debug, Clone, PartialEq)]
pub enum Object {
    Num(f64),
    Str(String),
    Bool(bool),
    Nil,
    ArithmeticError
}

impl fmt::Display for Object {
    fn fmt(&self, f: &mut fmt::Formatter ) -> fmt::Result{
        match self {
            Object::Num(x) => write!(f, "{x}"),
            Object::Str(x) => write!(f, "{x}"),
            Object::Bool(x) => {
                if *x {
                    write!(f, "true")
                } else {
                    write!(f, "false")
                }
            }
            Object::Nil => write!(f, "nil"),
            Object::ArithmeticError => panic!("Shouldn't be printing this???")
        }
    }
}

impl Sub for Object {
    type Output = Object;
    fn sub(self, other: Self) -> Object {
        match(self, other){
            (Object::Num(left), Object::Num(right)) => {
                Object::Num(left - right)
            }
            _ => Object::ArithmeticError
        }
    }
}