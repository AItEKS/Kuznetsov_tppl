namespace Pascal.Tests;

using Pascal.Lexer;
using Pascal.Parser;
using Pascal.Ast;

public class ParserTests
{
    [Fact]
    public void Parse_SimpleAssignmentStatement_ShouldCreateCorrectAst()
    {
        string input = "BEGIN x := 2 END.";
        Lexer lexer = new Lexer();
        Parser parser = new Parser(lexer);

        Node tree = parser.Parse(input);

        var programNode = Assert.IsType<ProgramNode>(tree);
        
        var compoundNode = Assert.IsType<Compound>(programNode.Block);
        
        var statement = Assert.Single(compoundNode.Children);

        var assignNode = Assert.IsType<Assign>(statement);

        var varNode = Assert.IsType<Var>(assignNode.Left);
        Assert.Equal("x", varNode.Name);

        var numberNode = Assert.IsType<Number>(assignNode.Right);
        Assert.Equal("2", numberNode.Token.Value);
    }
}