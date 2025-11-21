namespace Brainfuck.Core;

public class MachineState
{
    public byte[] Memory { get; } = new byte[30000];

    public int MemoryPointer { get; set; } = 0;

    public int InstructionPointer { get; set; } = 0;

    public int? Register { get; set; } = null;
}