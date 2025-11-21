namespace Brainfuck.Tests;

using Brainfuck.IO;

using System;
using System.IO;

public class ConsoleIOHandlerTests : IDisposable
{
    private readonly StringWriter _stringWriter = null!;
    private readonly TextWriter _originalOut = null!;
    private readonly TextReader _originalIn = null!;

    public ConsoleIOHandlerTests()
    {
        _originalOut = Console.Out;
        _stringWriter = new StringWriter();
        Console.SetOut(_stringWriter);

        _originalIn = Console.In;
    }

    [Fact]
    public void Write_ShouldWriteCharacterToConsoleOutput()
    {
        var handler = new ConsoleIOHandler();
        byte valueToWrite = 69;

        handler.Write(valueToWrite);

        string consoleOutput = _stringWriter.ToString();
        Assert.Equal("E", consoleOutput);
    }

    [Fact]
    public void Read_ShouldReadByteFromConsoleInput()
    {
        using (var stringReader = new StringReader("A"))
        {
            Console.SetIn(stringReader);
            var handler = new ConsoleIOHandler();

            byte readValue = handler.Read();

            byte expectedValue = 65;
            Assert.Equal(expectedValue, readValue);
        }
    }

    public void Dispose()
    {
        Console.SetOut(_originalOut);
        Console.SetIn(_originalIn);

        _stringWriter.Dispose();
    }
}