namespace CraftingInterpreters.Lox{

    public class Parser {

        public class ParseError : ApplicationException
        {
            public ParseError() : base() { }
            public ParseError(string message) : base(message) { }
            public ParseError(string message, Exception innerException) : base(message, innerException) { }
        }

        private readonly List<Token> tokens;
        private int current = 0;
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Stmt> parse() {
            List<Stmt> statements = new List<Stmt>();
            while( !isAtEnd() ){
                statements.Add(Statement());
            }
            return statements;
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Stmt Statement(){
            if( Match(TokenType.PRINT) ){
                return PrintStatement();
            }
            return ExpressionStatement();
        }

        private Stmt PrintStatement(){
            Expr value = Expression();
            Consume(TokenType.SEMICOLON,  "Expect ';' after expression." );
            return new Stmt.Print(value);
        }

        private Stmt ExpressionStatement(){
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON,  "Expect ';' after expression." );
            return new Stmt.Expression(expr);           
        }

        private Expr Assignment(){
            Expr expr = Equality();

            if( Match(TokenType.EQUAL) ){
                Token equals = previous();
                Expr values = Assignment();
                if (expr is Expr.Variable ){
                    
                }
            }

        }

        private Expr Equality()
        {
            Expr expr = Compare();

            while( Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL) ){
                Token opp = previous();
                Expr right = Compare();
                expr = new Expr.Binary(expr, opp, right);
            }

            return expr;

        }

        private Expr Compare()
        {
            Expr expr = Term();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL)) {
                Token opp = previous();
                Expr right = Term();
                expr = new Expr.Binary(expr, opp, right);
            }
            return expr;
        }

        private Expr Term(){
            Expr expr = Factor();
            while (Match(TokenType.MINUS, TokenType.PLUS)) {
                Token opp = previous();
                Expr right = Factor();
                expr = new Expr.Binary(expr, opp, right);
            }
            return expr;
        }

        private Expr Factor(){
            Expr expr = Unary();
            while (Match(TokenType.SLASH, TokenType.STAR)) {
                Token opp = previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, opp, right);
            }
            return expr;
        }

        private Expr Unary(){
            if (Match(TokenType.BANG, TokenType.MINUS)) {
                Token opp = previous();
                Expr right = Unary();
                return new Expr.Unary(opp, right);
            }

            return Primary();
        }

        private Expr Primary(){
            if (Match(TokenType.FALSE)) return new Expr.Literal(false);
            if (Match(TokenType.TRUE)) return new Expr.Literal(true);
            if (Match(TokenType.NIL)) return new Expr.Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Expr.Literal(previous().Literal);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            throw error(peek(), "Expect expression.");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Consume(TokenType type, string message){
            if( Check(type)){
                return Advance();
            }
            throw error(peek(), message);
        }

        private ParseError error(Token token, string message){
            Lox.Error(token, message);
            return new ParseError();
        }
        
        private void Synchronize() {
            Advance();

            while (!isAtEnd()) {
                if (previous().Type == TokenType.SEMICOLON) return;

                switch (peek().Type) {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                    return;
                }

                Advance();
            }
        }

        private bool Check(TokenType type)
        {
            if( isAtEnd() )
            {
                return false;
            }
            return peek().Type == type;

        }

        private Token Advance()
        {
            if( !isAtEnd() )
            {
                current++;
            }
            return previous();
        }

        private bool isAtEnd() {
            return peek().Type == TokenType.EOF;
        }

        private Token peek() {
            return tokens[current];
        }

        private Token previous() {
            return tokens[current-1];
        }

    }

}
