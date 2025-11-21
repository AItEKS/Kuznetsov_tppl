namespace Brainfuck.IO;

public interface IInputOutputHandler
{
    void Write(byte value);
    byte Read();
}