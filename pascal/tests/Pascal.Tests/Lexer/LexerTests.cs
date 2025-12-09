namespace Pascal.Tests;

using System.Data;
using Pascal.Lexer;
using Pascal.Token;

public class LexerTests
{
    [Fact]
    public void Constructor_InitCorrectDictionary()
    {
        Lexer lexer = new Lexer();
        Dictionary<string, Token> actualKeywords = lexer.GetReservedKeywords();

        Dictionary<string, Token> expectedResult = new Dictionary<string, Token>
        {
            { "BEGIN", new Token(TokenType.BEGIN, "BEGIN") },
            { "END", new Token(TokenType.END, "END") }
        };

        foreach (var pare in expectedResult)
        {
            Assert.True(actualKeywords.ContainsKey(pare.Key));
            Assert.Equal(pare.Value.Type, actualKeywords[pare.Key].Type);
            Assert.Equal(pare.Value.Value, actualKeywords[pare.Key].Value);
        }
    }

    [Fact]
    public void NextToken_ShouldParseCodeCorrectly()
    {
        Lexer lexer = new Lexer();
        string input = """
        BEGIN
            y := 2 * (4 + 5 - 2 / 2);
        END.
        """;

        lexer.SetText(input);

        Token token1 = lexer.NextToken();
        Assert.Equal(TokenType.BEGIN, token1.Type);

        Token token2 = lexer.NextToken();
        Assert.Equal(TokenType.ID, token2.Type);
        Assert.Equal("y", token2.Value);

        Token token3 = lexer.NextToken();
        Assert.Equal(TokenType.ASSIGN, token3.Type);
        Assert.Equal(":=", token3.Value);

        Token token4 = lexer.NextToken();
        Assert.Equal(TokenType.Number, token4.Type);
        Assert.Equal("2", token4.Value);

        Token token5 = lexer.NextToken();
        Assert.Equal(TokenType.Operator, token5.Type);
        Assert.Equal("*", token5.Value);

        Token token6 = lexer.NextToken();
        Assert.Equal(TokenType.LParen, token6.Type);
        Assert.Equal("(", token6.Value);

        Token token7 = lexer.NextToken();
        Assert.Equal(TokenType.Number, token7.Type);
        Assert.Equal("4", token7.Value);

        Token token8 = lexer.NextToken();
        Assert.Equal(TokenType.Operator, token8.Type);
        Assert.Equal("+", token8.Value);

        Token token9 = lexer.NextToken();
        Assert.Equal(TokenType.Number, token9.Type);
        Assert.Equal("5", token9.Value);

        Token token10 = lexer.NextToken();
        Assert.Equal(TokenType.Operator, token10.Type);
        Assert.Equal("-", token10.Value);

        Token token11 = lexer.NextToken();
        Assert.Equal(TokenType.Number, token11.Type);
        Assert.Equal("2", token11.Value);

        Token token12 = lexer.NextToken();
        Assert.Equal(TokenType.Operator, token12.Type);
        Assert.Equal("/", token12.Value);

        Token token13 = lexer.NextToken();
        Assert.Equal(TokenType.Number, token13.Type);
        Assert.Equal("2", token13.Value);

        Token token14 = lexer.NextToken();
        Assert.Equal(TokenType.RParen, token14.Type);
        Assert.Equal(")", token14.Value);

        Token token15 = lexer.NextToken();
        Assert.Equal(TokenType.SEMI, token15.Type);

        Token token16 = lexer.NextToken();
        Assert.Equal(TokenType.END, token16.Type);

        Token token17 = lexer.NextToken();
        Assert.Equal(TokenType.DOT, token17.Type);

        Token token18 = lexer.NextToken();
        Assert.Equal(TokenType.EOL, token18.Type);
    }

    [Fact]
    public void NextToken_WithSyntaxError_ShouldThrowEx()
    {
        Lexer lexer = new Lexer();
        string input = """
        #
        """;

        lexer.SetText(input);

        string actualEx = Assert.Throws<SyntaxErrorException>(() => lexer.NextToken()).ToString();
        string expectedEx = "System.Data.SyntaxErrorException: Unknown or invalid character: '#'";

        Assert.Contains(expectedEx, actualEx);
    }
} 