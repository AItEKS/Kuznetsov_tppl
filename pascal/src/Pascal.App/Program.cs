using Pascal.Parser;
using Pascal.Lexer;
using Pascal.Interpreter;

var text = """
BEGIN
    y := 2;
    BEGIN
        a := 3;
        b := 10 + a + 10 * y / 4;
        c := a - b;
    END;
    x := 11;
END.
""";

var lexer = new Lexer();
var parser = new Parser(lexer);
Console.WriteLine(parser.Parse(text));

var interpreter = new Interpreter();
Console.WriteLine(interpreter.Eval(text));

Console.WriteLine("Таблица символов после выполнения:");
foreach (var pair in interpreter.GetSymbolTable())
{
    Console.WriteLine($"{pair.Key} = {pair.Value}");
}