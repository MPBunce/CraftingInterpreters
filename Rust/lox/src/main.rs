use std::env;
use std::io::{ self, BufRead};

fn main() {
    let args: Vec<String> = env::args().collect();
    println!("{}", &args[1]);
    if args.len() > 2 {
        println!("Usage: lox-ast script");
    }else if args.len() == 2 {
        run_file(&args[1]).expect("Could not run file");
    }else{
        run_prompt();
    }
}

fn run_file(filename: &str)-> io::Result<()> {

    let buf = std::fs::read_to_string(filename)?;
    run(&buf);
    Ok(())
}

fn run_prompt(){
    let stdin = io::stdin();
    print!("> ");
    for line in stdin.lock().lines(){
        if let Ok(line) = line {
            if line.is_empty(){
                break;
            } 
            run(line.as_str());
        }else {
            break
        }
    }
}

fn run(source: &str){
    let scanner = Scanner { source };
    let tokens = scanner.scan_tokens();
    
    for token in tokens {
        println!("{}", token);
    }

}