namespace Brainfuck;

using Brainfuck.Parsing;
using Brainfuck.IO;
using Brainfuck.Core;

using System;
using System.IO;


public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Moooo COW Interpreter ooooM\n");

        string fileName = args.Length > 0 ? args[0] : "";

        if (!File.Exists(fileName))
        {
            Console.WriteLine($"Файл '{fileName}' не найден!");
            Console.WriteLine("Использование: dotnet run [имя_файла.cow]");
            return;
        }

        try
        {
            string code = File.ReadAllText(fileName);
            
            var parser = new CowParser();
            var commands = parser.Parse(code);

            var interpreter = new CowInterpreter(new ConsoleIOHandler());
            
            Console.WriteLine("РЕЗУЛЬТАТ:");
            interpreter.Execute(commands);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}");
        }
    }
}