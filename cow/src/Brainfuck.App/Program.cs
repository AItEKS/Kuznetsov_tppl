// Program.cs
using Brainfuck.Core;
using Brainfuck.IO;
using Brainfuck.Parsing;

namespace Brainfuck;

class Program
{
    static void Main(string[] args)
    {
        var runner = new ProgramRunner();
        int exitCode = runner.Run(args);
        Environment.Exit(exitCode);
    }
}

public class ProgramRunner
{
    private readonly TextWriter _output;
    private readonly Func<string, bool> _fileExists;
    private readonly Func<string, string> _readFile;

    public ProgramRunner(
        TextWriter? output = null,
        Func<string, bool>? fileExists = null,
        Func<string, string>? readFile = null)
    {
        _output = output ?? Console.Out;
        _fileExists = fileExists ?? File.Exists;
        _readFile = readFile ?? File.ReadAllText;
    }

    public int Run(string[] args)
    {
        _output.WriteLine("Moooo COW Interpreter ooooM\n");

        string fileName = args.Length > 0 ? args[0] : "program.cow";

        if (!_fileExists(fileName))
        {
            _output.WriteLine($"Файл '{fileName}' не найден!");
            _output.WriteLine("Использование: dotnet run [имя_файла.cow]");
            return 1;
        }

        try
        {
            string code = _readFile(fileName);
            var parser = new CowParser();
            var commands = parser.Parse(code);

            var ioHandler = new ConsoleIOHandler();
            var interpreter = new CowInterpreter(ioHandler);

            _output.WriteLine("РЕЗУЛЬТАТ:");
            interpreter.Execute(commands);

            return 0;
        }
        catch (Exception ex)
        {
            _output.WriteLine($"\nОшибка: {ex.Message}");
            _output.WriteLine(ex.StackTrace);
            return 1;
        }
    }
}