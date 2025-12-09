namespace Pascal.Token;

public enum TokenType
{
    Number,
    Operator,
    LParen,
    RParen,
    EOL,
    BEGIN,
    END,
    DOT,
    SEMI,
    ID,
    ASSIGN
}

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    
    public Token(TokenType type, string value)
    {
        this.Type = type;
        this.Value = value;
    }

    public override string ToString()
    {
        return $"{GetType().Name}(Type={Type}, Value='{Value}')";
    }
}