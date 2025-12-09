namespace Pascal.Tests;

using Pascal.Interpreter;

public class InterpreterTests
{
    [Fact]
    public void Eval_SimpleAssignment_ShouldUpdateSymbolTable()
    {
        Interpreter interpreter = new Interpreter();
        string input = """
        BEGIN
            y := 2 * 1;
            x := 5 / 1;
            z := y + x - 1;
        END.
        """;

        interpreter.Eval(input);

        Dictionary<string, float> actualSymbTable = interpreter.GetSymbolTable();
        Dictionary<string, float> expectedSymbTable = new Dictionary<string, float>
        {
            ["y"] = 2f,
            ["x"] = 5f,
            ["z"] = 6f,
        };

        Assert.Equal(expectedSymbTable, actualSymbTable);
    }

    [Fact]
    public void Eval_SimpleAssignmentWithError_ShouldThrowException()
    {
        Interpreter interpreter = new Interpreter();
        string input = """
        BEGIN
            x := 5;
            z := y + x;
        END.
        """;

        var actualEx = Assert.Throws<KeyNotFoundException>(() => interpreter.Eval(input));
        
        Assert.Equal("Variable 'y' is not defined.", actualEx.Message);
    }

    [Fact]
    public void Eval_SimpleAssignmentWithZeroDiv_ShouldThrowException()
    {
        Interpreter interpreter = new Interpreter();
        string input = """
        BEGIN
            x := 5 / 0;
        END.
        """;

        var actualEx = Assert.Throws<DivideByZeroException>(() => interpreter.Eval(input));
        
        Assert.Equal("An attempt to divide by zero", actualEx.Message);
    }

    [Fact]
    public void Vizit_ShouldDoNothing()
    {
        NodeVizitor nodeVizitor = new NodeVizitor();
        Exception? exception = null;
        
        try
        {
            nodeVizitor.Vizit();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.Null(exception);
    }
}