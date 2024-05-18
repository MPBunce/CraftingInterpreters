mod error;
use error::*;
mod scanner;
use scanner::*;
mod token;
mod token_type;

use std::env;
use std::io::{ self, BufRead, Write, stdout};


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
        Err(_) => {
            //m.report( "".to_string());
            std::process::exit(65);
        }
    }
    Ok(())

}

fn run_prompt(){
    let stdin = io::stdin();
    print!("> ");
    let _ = stdout().flush();
    for line in stdin.lock().lines(){
        if let Ok(line) = line {
            if line.is_empty(){
                break;
            } 
            match run(line) {
                Ok(_) => {}
                Err(_) => {
                    //Ignore
                }
            }

        }else {
            break
        }
        print!("> ");
        let _ = stdout().flush();
    }
}

fn run(source: String)-> Result<(), LoxError>{
    println!("Running...");
    let mut scanner = Scanner::new(source);
    let tokens = scanner.scan_tokens()?;
    println!("Done Scanning...");
    for token in tokens {
        println!("{:?}", token);
    }
    Ok(())
}




