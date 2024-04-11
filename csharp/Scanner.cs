using System;
using System.Collections.Generic;
using System.IO;

namespace CraftingInterpreters.Lox
{
    public class Token
    {
        public string Value { get; set; }

        public Token(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class Scanner
    {
        private StreamReader _reader;

        public Scanner(StreamReader reader)
        {
            _reader = reader;
        }

        public List<Token> ScanTokens()
        {
            List<Token> tokens = new List<Token>();
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                // Tokenize the line and add tokens to the list
                string[] words = line.Split(' ');
                foreach (string word in words)
                {
                    tokens.Add(new Token(word));
                }
            }
            return tokens;
        }
    }

}