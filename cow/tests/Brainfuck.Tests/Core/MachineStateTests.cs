namespace Brainfuck.Tests;

using Brainfuck.Core;

public class MachineStateTests
{
    [Fact]
    public void Constructor_InitializesMemoryWith30000Bytes()
    {
        var state = new MachineState();

        Assert.NotNull(state.Memory);
        Assert.Equal(30000, state.Memory.Length);
    }
}