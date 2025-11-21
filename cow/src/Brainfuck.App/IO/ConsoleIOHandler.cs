namespace Brainfuck.IO;

public class ConsoleIOHandler : IInputOutputHandler
{
    public void Write(byte value)
    {
        Console.Write((char)value);
    }

    public byte Read()
    {
        var input = Console.Read();
        return (byte)input;
    }
}