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
        Node left = new Number(new Token(TokenType.Number, "1"));
        Node right = new Number(new Token(TokenType.Number, "5"));
        Token op = new Token(TokenType.Operator, "+");

        BinOp binOp = new BinOp(left, op, right);

        string overrideString = binOp.ToString();

        string expectedString = $"BinOp+(Number(Token(Type=Number, Value='1')'), Number(Token(Type=Number, Value='5')'))";

        Assert.Equal(expectedString, overrideString); 
    }

    [Fact]
    public void CompoundToString_CorrectOverride()
    {
        var numberNode = new Number(new Token(TokenType.Number, "7"));

        var binOpNode = new BinOp(
            new Number(new Token(TokenType.Number, "2")),
            new Token(TokenType.Operator, "*"),
            new Number(new Token(TokenType.Number, "3"))
        );

        var compound = new Compound();
        compound.Children.Add(numberNode);
        compound.Children.Add(binOpNode);

        string overrideString = compound.ToString();

        string expectedString = "Compound(Number(Token(Type=Number, Value='7')'), BinOp*(Number(Token(Type=Number, Value='2')'), Number(Token(Type=Number, Value='3')')))";

        Assert.Equal(expectedString, overrideString);
    }

    [Fact]
    public void VarToString_CorrectOverride()
    {
        Token token = new Token(TokenType.ID, "x");

        Var var = new Var(token);

        string overrideString = var.ToString();

        string expectedString = $"Var(Name=x)";

        Assert.Equal(expectedString, overrideString);
        Assert.Equal(token, var.Token);
    }

    [Fact]
    public void AssignToString_CorrectOverride()
    {
        Var left = new Var(new Token(TokenType.ID, "x"));
        Node right = new Number(new Token(TokenType.Number, "15"));

        Assign assign = new Assign(left, right);

        string overrideString = assign.ToString();

        string expectedString = $"Assign(Var(Name=x) := Number(Token(Type=Number, Value='15')'))";

        Assert.Equal(expectedString, overrideString); 
    }

    [Fact]
    public void NoOpToString_CorrectOverride()
    {
        NoOp noOp = new NoOp();
        
        Assert.Equal("NoOp", noOp.ToString());
    }

    [Fact]
    public void ProgramNodeToString_CorrectOverride()
    {
        var compoundBlock = new Compound();
        compoundBlock.Children.Add(
            new BinOp(
                new Number(new Token(TokenType.Number, "10")),
                new Token(TokenType.Operator, "+"),
                new Number(new Token(TokenType.Number, "20"))
            )
        );

        var programNode = new ProgramNode(compoundBlock);

        string overrideString = programNode.ToString();

        string expectedString = "Program(Name=Default, Compound(BinOp+(Number(Token(Type=Number, Value='10')'), Number(Token(Type=Number, Value='20')'))))";

        Assert.Equal(expectedString, overrideString);
    }
}