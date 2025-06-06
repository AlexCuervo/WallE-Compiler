public enum TokenType
{
    number,
    op,
    assign,
    functionCall,
    goTo,
    param,
    group,
    exclude,
    id,
    text,
    boolean,
    keyword,
    error,
    end,
    instructions,
    program,
    epsilon
}


public class Token(TokenType Type, string Literal, int Row, int Column)
{
    public TokenType type = Type;
    public string literal = Literal;
    public int row = Row;
    public int column = Column;

}
