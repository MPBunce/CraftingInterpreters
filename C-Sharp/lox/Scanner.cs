using System.Text.RegularExpressions;

namespace CraftingInterpreters.Lox{

    public class Scanner {
        public readonly string source;
        private int start = 0;
        private int current = 0;
        private int line = 1;

        private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>(){
            {"and", TokenType.AND},
            {"class", TokenType.CLASS},
            {"else", TokenType.ELSE},
            {"false", TokenType.FALSE},
            {"for", TokenType.FOR},
            {"fun", TokenType.FUN},
            {"if", TokenType.IF},
            {"nil", TokenType.NIL},
            {"or", TokenType.OR},
            {"print", TokenType.PRINT},
            {"return", TokenType.RETURN},
            {"super", TokenType.SUPER},
            {"this", TokenType.THIS},
            {"true", TokenType.TRUE},
            {"var", TokenType.VAR},
            {"while", TokenType.WHILE}
        };

        private readonly List<Token> tokens = new List<Token>();
        public Scanner(string source){
            this.source = source;
        }
        public List<Token> ScanTokens(){
            while( !IsAtEnd() ){
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        void ScanToken(){
            char c = Advance();
            switch (c) {
                case '(' :
                    addToken(TokenType.LEFT_PAREN);
                    break;
                case ')' :
                    addToken(TokenType.RIGHT_PAREN);
                    break;
                 case '{' :
                    addToken(TokenType.LEFT_BRACE);
                    break;
                case '}' :
                    addToken(TokenType.RIGHT_BRACE);
                    break;
                case ',' :
                    addToken(TokenType.COMMA);
                    break;
                case '.' :
                    addToken(TokenType.DOT);
                    break;
                case '-' :
                    addToken(TokenType.MINUS);
                    break;
                case '+' :
                    addToken(TokenType.PLUS);
                    break;
                case ';' :
                    addToken(TokenType.SEMICOLON);
                    break;
                case '*' :
                    addToken(TokenType.STAR);
                    break;
                case '!':
                    addToken( Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    addToken( Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;   
                case '<':
                    addToken( Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;  
                case '>':
                    addToken( Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;  
                case '/':
                    if (Match('/')){
                        while(Peek() != '\n' && !IsAtEnd()){
                            Advance();
                        }
                    }else{
                        addToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    Str();
                    break;
                case 'o':
                    if ( Match('r') ){
                        addToken(TokenType.OR);
                    }
                    break;
                default:
                    if( IsDigit(c) ){
                        Number();
                    } else if (IsAlpha(c)) {
                        Identifier();
                    }else{
                        Lox.Error(line, "Unexpected character.e");
                    }
                    break;
            }

        }

        void Identifier(){
            while( IsAlphaNumeric(Peek()) ){
                Advance();
            }

            TokenType type;
            string text = source[start..current];
            
            if(keywords.ContainsKey(text)){
                type = keywords[text];
            }else{
                type = TokenType.IDENTIFIER;
            }
            addToken(type);
        }
        
        bool IsAlphaNumeric(char c){
            return IsAlpha(c) || IsDigit(c);
        }
        bool IsAlpha(char c){
            return (c >= 'a' && c <= 'z' ) || (c >= 'A' && c <= 'Z') || c == '_';
        }

        void Number(){
            while( IsDigit(Peek()) ){
                Advance();
            }
            if( Peek() == '.' && IsDigit(PeekNext()) ){
                Advance();
                while(IsDigit(Peek())){
                    Advance();
                }
            }
            addToken(TokenType.NUMBER, Double.Parse( source[start..current] ));
        }

        void Str(){
            while( Peek() != '"' && !IsAtEnd()){
                if( Peek() == '\n'){
                    line++;
                }
                Advance();
            }
            //Console.WriteLine("loop");
            if( IsAtEnd() ){
                Lox.Error(line, "Unterminated string.");
                return;
            }
            Advance();

            String value = source[(start+1)..(current-1)];
            //Console.WriteLine("string val:" + value);
            addToken(TokenType.STRING, value);
        }

        bool Match(char c){
            if( IsAtEnd() ){
                return false;
            }
            if( source[current] != c){
                return false;
            }
            current++;
            return true;
        }
        
        char Peek(){

            if(IsAtEnd()){
                return '\0';
            }
            return source[current];
        }

        char PeekNext(){
            if( (current+1) >= source.Length){
                return '\0';
            }
            return source[current+1];
        }

        bool IsDigit(char c){
            return c >= '0' && c <= '9';
        }

        void addToken(TokenType type){
            addToken(type, null);
        }
        void addToken(TokenType type, Object literal){
            String text = source[start..current];
            tokens.Add(new Token(type, text, literal, line) );
        }
        char Advance(){
            return source[current++];
        }

        bool IsAtEnd(){
            return current >= source.Length;
        }

    }

}