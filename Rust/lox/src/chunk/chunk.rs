pub enum OpCode {
    OpReturn,
}

pub struct Chunk {
    code: Vec<u8>,
}

impl Chunk {
    pub fn init_chunk() -> Self{
        return Chunk {code: Vec::new()}
    }
    pub fn write_chunk(&mut self, byte: u8) {
        self.code.push(byte);
    }
    pub fn disassemble_chunk(){
        
    }
}