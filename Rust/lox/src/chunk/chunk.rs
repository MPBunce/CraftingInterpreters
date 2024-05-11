pub enum OpCode {
    OpReturn,
}

pub struct Chunk {
    count: i32,
    capacity: i32,
    code: Option<*mut u8>,
}

impl Chunk {
    pub fn init_chunk() -> Self{
        return Chunk {count: 0, capacity: 0, code: None}
    }
    pub fn free_chunk(&mut self){
        return;
    }
    pub fn write_chunk(&mut self, byte: u8){
        if self.capacity < self.count + 1 {
            let old_capacity = self.capacity;

        }
    }
}