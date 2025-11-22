namespace Brainfuck.Tests;

using Brainfuck.IO;
using Brainfuck.Core;
using Brainfuck.Parsing;

using Moq;

[Collection("SystemTests")]
public class CowInterpreterTests
{
    private readonly Mock<IInputOutputHandler> _mockIO;
    private readonly CowInterpreter _interpreter;
    
    public CowInterpreterTests()
    {
        _mockIO = new Mock<IInputOutputHandler>();
        _interpreter = new CowInterpreter(_mockIO.Object);
    }

    [Fact]
    public void Increment_IncreasesValueInMemory()
    {
        var commands = new List<CowCommand>
        {
            CowCommand.Increment,
            CowCommand.InOrOut
        };

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Write(1), Times.Once);
    }

    [Fact]
    public void Decrement_ReducesValueInMemory()
    {
        var commands = new List<CowCommand>
        {
            CowCommand.Increment,
            CowCommand.Increment,
            CowCommand.Decrement,
            CowCommand.InOrOut
        };

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Write(1), Times.Once);
    }

    [Fact]
    public void Reset_ResetValue()
    {
        var commands = new List<CowCommand>
        {
            CowCommand.Increment,
            CowCommand.Increment,
            CowCommand.Reset,
            CowCommand.InOrOut
        };

        _mockIO.Setup(io => io.Read()).Returns(42);

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Read(), Times.Once);
    }

    [Fact]
    public void MoveNext_MovesPointerToRight()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.Increment,      // ячейка 0 = 1
            CowCommand.MoveNext,       // переход на ячейку 1
            CowCommand.Increment,      // ячейка 1 = 1
            CowCommand.Increment,      // ячейка 1 = 2
            CowCommand.InOrOut         // вывод ячейки 1
        };

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Write(2), Times.Once);
    }

    [Fact]
    public void MovePrevious_MovesPointerToLeft()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.MoveNext,       // на ячейку 1
            CowCommand.Increment,      // ячейка 1 = 1
            CowCommand.MovePrevious,   // на ячейку 0
            CowCommand.Increment,      // ячейка 0 = 1
            CowCommand.InOrOut         // вывод ячейки 0
        };

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Write(1), Times.Once);
    }

    [Fact]
    public void Loop_AssignmentIsBeingPerformed()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.Increment,      // ячейка 0 = 1
            CowCommand.Increment,      // ячейка 0 = 2
            CowCommand.Increment,      // ячейка 0 = 3
            CowCommand.LoopStart,      // [
            CowCommand.Decrement,      // уменьшаем счетчик
            CowCommand.LoopEnd         // ]
        };
        
        _interpreter.Execute(commands);
        
        Assert.True(true);
    }

    [Fact]
    public void Loop_SkippingValueZero()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.LoopStart,      // [
            CowCommand.Increment,      // не должно выполниться
            CowCommand.LoopEnd,        // ]
            CowCommand.InOrOut         // если 0, то читает
        };
        
        _mockIO.Setup(io => io.Read()).Returns(99);
        
        _interpreter.Execute(commands);
        
        _mockIO.Verify(io => io.Read(), Times.Once);
    }

    [Fact]
    public void Input_ReadsValuesOfMemory()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.Input,
            CowCommand.InOrOut
        };

        _mockIO.Setup(io => io.Read()).Returns(42);

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Read(), Times.Once);
        _mockIO.Verify(io => io.Write(42), Times.Once);
    }

    [Fact]
    public void InOrOut_ReadIfZero()
    {
        var commands = new List<CowCommand>
        {
            CowCommand.InOrOut
        };
        _mockIO.Setup(io => io.Read()).Returns(10);

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Read(), Times.Once);
    }

    [Fact]
    public void InOrOut_WriteIfNoZero()
    {
        var commands = new List<CowCommand>
        {
            CowCommand.Increment,
            CowCommand.Increment,
            CowCommand.InOrOut
        };

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Write(2), Times.Once);
    }

    [Fact]
    public void NestedCycles_WorkCorrectly()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.Increment,      // счетчик внешнего = 2
            CowCommand.Increment,
            CowCommand.LoopStart,      // [
                CowCommand.MoveNext,   
                CowCommand.Increment,  // счетчик внутреннего = 2
                CowCommand.Increment,
                CowCommand.LoopStart,  // [
                    CowCommand.Decrement,
                CowCommand.LoopEnd,    // ]
                CowCommand.MovePrevious,
                CowCommand.Decrement,
            CowCommand.LoopEnd         // ]
        };
        
        _interpreter.Execute(commands);
        
        Assert.True(true);
    }

    [Fact]
    public void EmptyProgram_DoesNotCauseErrors()
    {
        var commands = new List<CowCommand>();
        
        _interpreter.Execute(commands);
        
        Assert.True(true);
    }

    [Fact]
    public void Output_WriteValue()
    {
        var commands = new List<CowCommand> 
        { 
            CowCommand.Input,
            CowCommand.Output
        };

        _mockIO.Setup(io => io.Read()).Returns(42);

        _interpreter.Execute(commands);

        _mockIO.Verify(io => io.Read(), Times.Once);
    }
}
