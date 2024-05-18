use std::env::args;
use std::fs::File;
use std::io::Write;
use std::io;

#[derive(Debug)]
struct TreeType {
    base_class_name: String,
    class_name: String,
    fields: Vec<String>
}

fn main() -> io::Result<()> {

    let args: Vec<String> = args().collect();
    if args.len() != 2 {
        println!("Usage: lox-ast script needs output directory");
        std::process::exit(64);
    }

    let output_dir = args.get(1).unwrap();

    let ast = vec![
        "Binary   : Box<Expr> left, Token operator, Box<Expr> right".to_string(),
        "Grouping : Box<Expr> expression".to_string(),
        "Literal  : Object value".to_string(),
        "Unary    : Token operator, Box<Expr> right".to_string()
    ];

    define_ast(output_dir, &"Expr".to_string(), ast)
}

fn define_ast(output_dir: &String, base_name: &String, types:  Vec<String>) -> io::Result<()> {
    let path = format!("{output_dir}/{}.rs", base_name.to_lowercase() );
    let mut file = File::create(path)?;
    let mut tree_types = Vec::new();

    write!(file, "{}", "use crate::error::*;\n")?;
    write!(file, "{}", "use crate::token::*;\n\n\n")?;

    for ttype in types {

        let (base_class_name, args) = ttype.split_once(":").unwrap();
        let class_name = format!("{}{}", base_class_name.trim(), base_name);
        let args_split = args.split(",");
        let mut fields = Vec::new();

        for arg in args_split {
            let (t2type, name) = arg.trim().split_once(" ").unwrap();
            fields.push( format!("{}: {}", name, t2type) );
        }

        tree_types.push(
            TreeType {
                base_class_name: base_class_name.to_string(),
                class_name,
                fields
            }
        );
    }

    write!(file, "pub enum {base_name} {{ \n")?;
    for t in &tree_types {
        write!(file, "    {}({}),\n", t.base_class_name, t.class_name)?;
    }
    write!(file, "}}\n\n")?;

    Ok(())
}