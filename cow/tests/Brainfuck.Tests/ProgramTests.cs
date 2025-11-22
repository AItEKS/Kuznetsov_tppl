namespace Brainfuck.Tests;

using System;
using System.IO;
using Xunit;

using Brainfuck;

[Collection("SystemTests")]
public class ProgramTests
{
    [Fact]
    public void Main_WhenFileDoesNotExist_PrintsErrorMessage()
    {
        var stringWriter = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            Program.Main(new[] { "missing_file.cow" });

            var output = stringWriter.ToString();
            Assert.Contains("не найден", output);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
    
    [Fact]
    public void Main_WhenNoArgs_PrintsUsage()
    {
        var stringWriter = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            Program.Main(Array.Empty<string>());

            var output = stringWriter.ToString();
            Assert.Contains("Использование:", output);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}