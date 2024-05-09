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
                statements.Add( Declaration() );
            }
            return statements;
        }

        private Stmt Declaration()
        {
            try {
                if( Match(TokenType.VAR) ){
                    return VarDeclaration();
                }
                return Statement();
            }
            catch (ParseError error){
                Synchronize();
                return null;
            }
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Stmt Statement(){
            if( Match(TokenType.FOR) ){
                return ForStatement();
            }
            if( Match(TokenType.IF) ){
                return IfStatement();
            }
            if( Match(TokenType.PRINT) ){
                return PrintStatement();
            }
            if( Match(TokenType.WHILE) ){
                return WhileStatement();
            }
            if( Match(TokenType.LEFT_BRACE) ){
                return new Stmt.Block(Block());
            }
            return ExpressionStatement();
        }

        private Stmt ForStatement() {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'for'.");

            Stmt initializer;
            if (Match(TokenType.SEMICOLON)) {
                initializer = null;
            } else if (Match(TokenType.VAR)) {
                initializer = VarDeclaration();
            } else {
                initializer = ExpressionStatement();
            }

            Expr condition = null;
            if (!Check(TokenType.SEMICOLON)) {
                condition = Expression();
            }
            Consume(TokenType.SEMICOLON, "Expect ';' after loop condition.");

            Expr increment = null;
            if (!Check(TokenType.RIGHT_PAREN)) {
                increment = Expression();
            }
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after for clauses.");

            Stmt body = Statement();
            if (increment != null){
                body = new Stmt.Block(new List<Stmt> { body, new Stmt.Expression(increment) });
            }
            if (condition == null) condition = new Expr.Literal(true);
            body = new Stmt.While(condition, body);

            if (initializer != null)
            {
                body = new Stmt.Block(new List<Stmt> { initializer, body });
            }

            return body;
        }

        private Stmt IfStatement(){
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
            Expr condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition."); 

            Stmt thenBranch = Statement();
            Stmt elseBranch = null;
            if (Match(TokenType.ELSE)) {
                elseBranch = Statement();
            }

            return new Stmt.If(condition, thenBranch, elseBranch);
        }

        private Stmt WhileStatement(){
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'while'.");
            Expr condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
            Stmt body = Statement();

            return new Stmt.While(condition, body);
        }

        private Stmt PrintStatement(){
        
            Expr value = Expression();
            Consume(TokenType.SEMICOLON,  "Expect ';' after expression." );
            return new Stmt.Print(value);
        }

        private Stmt VarDeclaration(){
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");
            Expr initializer = null;
            
            if( Match(TokenType.EQUAL) ){
                initializer = Expression();
            }
            
            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);

        }

        private Stmt ExpressionStatement(){
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON,  "Expect ';' after expression." );
            return new Stmt.Expression(expr);           
        }

        private List<Stmt> Block(){
            List<Stmt> statements = new List<Stmt>();

            while (!Check(TokenType.RIGHT_BRACE) && !isAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;

        }

        private Expr Assignment(){
            Expr expr = Or();

            if( Match(TokenType.EQUAL) ){
                Token equals = previous();
                Expr values = Assignment();
                if (expr is Expr.Variable ){
                    Token name = ((Expr.Variable)expr).name;
                    return new Expr.Assign(name, values);
                }

                error(equals, "Invalid assignment target.");

            }

            return expr;

        }

        private Expr Or(){
            Expr expr = And();

            while( Match(TokenType.OR) ){
                Token opp = previous();
                Expr right = And();
                expr = new Expr.Logical(expr, opp, right);
            }

            return expr;
        }

        private Expr And(){
            Expr expr = Equality();

            while( Match(TokenType.AND) ){
                Token opp = previous();
                Expr right = Equality();
                expr = new Expr.Logical(expr, opp, right);
            }
            
            return expr;
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

            if (Match(TokenType.IDENTIFIER)) {
                return new Expr.Variable(previous());
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
