public enum TokenType
{
    number,
    op,
    group,
    exclude,
    id,
    keyword,
    error,
    end,
    epsilon
}


public class Token(TokenType Type, string Literal, int Row, int Column)
{
    public TokenType type = Type;
    public string literal = Literal;
    public int row = Row;
    public int column = Column;

}
