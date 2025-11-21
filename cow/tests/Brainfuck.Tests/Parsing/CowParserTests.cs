namespace Brainfuck.Tests;

using Brainfuck.Parsing;

public class CowParserTests
{
    [Fact]
    public void Parse_SimpleCommands_ReturnsCommands()
    {
        var parser = new CowParser();
        var sourceCode = "MoO MOo moO";
        
        var expectedCommands = new List<CowCommand>
        {
            CowCommand.Increment,
            CowCommand.Decrement,
            CowCommand.MoveNext
        };

        var parseCommands = parser.Parse(sourceCode);

        Assert.Equal(expectedCommands, parseCommands);
    }

    [Fact]
    public void Parse_WithComments_ReturnsCommands()
    {
        var parser = new CowParser();
        var commentedCode = "MoO MOo moO ; here some comment\n\tHere comment\nMOO moo ; here another comment\n";

        var expectedCommands = new List<CowCommand>
        {
            CowCommand.Increment,
            CowCommand.Decrement,
            CowCommand.MoveNext,
            CowCommand.LoopStart,
            CowCommand.LoopEnd
        };

        var parseCommands = parser.Parse(commentedCode);

        Assert.Equal(expectedCommands, parseCommands);
    }
}
