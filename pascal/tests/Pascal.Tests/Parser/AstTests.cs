namespace Pascal.Tests;

using Pascal.Token;
using Pascal.Ast;

public class AstTests
{
    [Fact]
    public void NumberToString_CorrectOverride()
    {
        Token token = new Token(TokenType.Number, "25");
        Number number = new Number(token);

        string overrideString = number.ToString();

        string expectedString = $"Number(Token(Type=Number, Value='25')')";

        Assert.Equal(expectedString, overrideString); 
    }

    [Fact]
    public void BinOpToString_CorrectOverride()
    {
        Token token1 = new Token(TokenType.Number, "1");
        Node left = new Number(token1);

        Token token2 = new Token(TokenType.Number, "5");
        Node right = new Number(token2);

        Token token3 = new Token(TokenType.Operator, "+");

        BinOp binOp = new BinOp(left, token3, right);

        string overrideString = binOp.ToString();

        string expectedString = $"BinOp+(Number(Token(Type=Number, Value='1')'), Number(Token(Type=Number, Value='5')'))";

        Assert.Equal(expectedString, overrideString); 
    }
}