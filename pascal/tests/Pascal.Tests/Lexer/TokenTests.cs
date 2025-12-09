namespace Pascal.Tests;

using Pascal.Token;

public class TokenTests
{
    [Fact]
    public void ToString_CorrectOverride()
    {
        Token token = new Token(TokenType.Number, "25");

        string overrideString = token.ToString();

        string expectedString = $"Token(Type=Number, Value='25')";

        Assert.Equal(expectedString, overrideString); 
    }
}