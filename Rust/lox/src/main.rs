//Modules
mod chunk;
mod memory;

//Crates
use std::env;
use crate::chunk::chunk::{OpCode, Chunk};

fn main() {
    let args: Vec<String> = env::args().collect();

    println!("Compiling....\n");
    let mut chunk = Chunk::init_chunk();
    // chunk.write_chunk(OpCode::OpReturn);
    // chunk.free_chunk();

    return;
}
