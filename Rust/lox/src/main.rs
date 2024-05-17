mod error;
use error::*;
mod scanner;
use scanner::*;
mod token;
mod token_type;

use std::env;
use std::io::{ self, BufRead, BufReader, Write, stdout};


fn main() {
    let args: Vec<String> = env::args().collect();
    if args.len() > 2 {
        println!("Usage: lox-ast script");
        std::process::exit(64);
    }else if args.len() == 2 {
        run_file(&args[1]).expect("Could not run file");
    }else{
        run_prompt();
    }
}

fn run_file(filename: &str)-> io::Result<()> {

    let buf = std::fs::read_to_string(filename)?;
    match run(buf){
        Ok(_) => {},
        Err(m) => {
            m.report( "".to_string());
            std::process::exit(65);
        }
    }
    Ok(())
}

fn run_prompt(){
    let stdin = io::stdin();
    print!("> ");
    stdout().flush();
    for line in stdin.lock().lines(){
        if let Ok(line) = line {
            if line.is_empty(){
                break;
            } 
            match run(line) {
                Ok(_) => {}
                Err(e) => {
                    e.report("".to_string());
                }
            }

        }else {
            break
        }
        print!("> ");
        stdout().flush();
    }
}

fn run(source: String)-> Result<(), LoxError>{
    let mut scanner = Scanner::new(source);
    let tokens = scanner.scan_tokens();
    
    for token in tokens {
        println!("{:?}\n", token);
    }
    Ok(())
}




