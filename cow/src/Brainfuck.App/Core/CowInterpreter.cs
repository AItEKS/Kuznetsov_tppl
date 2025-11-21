using Brainfuck.IO;
using Brainfuck.Parsing;

namespace Brainfuck.Core;

public class CowInterpreter
{
    private readonly IInputOutputHandler _ioHandler;
    private readonly Dictionary<int, int> _jumpMap = new();

    public CowInterpreter(IInputOutputHandler ioHandler)
    {
        _ioHandler = ioHandler;
    }

    public void Execute(List<CowCommand> commands)
    {
        var state = new MachineState();
        BuildJumpMap(commands);

        while (state.InstructionPointer < commands.Count)
        {
            var command = commands[state.InstructionPointer];
            state.InstructionPointer = ExecuteCommand(command, state, commands);
        }
    }

    private int ExecuteCommand(CowCommand command, MachineState state, List<CowCommand> commands)
    {
        int nextInstructionPointer = state.InstructionPointer + 1;

        switch (command)
        {
            case CowCommand.Increment:
                state.Memory[state.MemoryPointer]++;
                break;
            case CowCommand.Decrement:
                state.Memory[state.MemoryPointer]--;
                break;
            case CowCommand.MoveNext:
                state.MemoryPointer++;
                break;
            case CowCommand.MovePrevious:
                state.MemoryPointer--;
                break;
            case CowCommand.LoopStart:
                if (state.Memory[state.MemoryPointer] == 0)
                {
                    nextInstructionPointer = _jumpMap[state.InstructionPointer] + 1;
                }
                break;
            case CowCommand.LoopEnd:
                if (state.Memory[state.MemoryPointer] != 0)
                {
                    nextInstructionPointer = _jumpMap[state.InstructionPointer];
                }
                break;
            case CowCommand.Output:
                Console.Write(state.Memory[state.MemoryPointer] + " ");
                break;
            case CowCommand.Input:
                state.Memory[state.MemoryPointer] = _ioHandler.Read();
                break;
            case CowCommand.Execute:
                var targetIndex = state.Memory[state.MemoryPointer];
                if (targetIndex >= 0 && targetIndex < commands.Count)
                {
                    nextInstructionPointer = targetIndex;
                }
                break;
            case CowCommand.InOrOut:
                if (state.Memory[state.MemoryPointer] == 0)
                {
                    state.Memory[state.MemoryPointer] = _ioHandler.Read();
                } 
                else
                {
                    _ioHandler.Write(state.Memory[state.MemoryPointer]);
                }
                break;
            case CowCommand.Reset:
                state.Memory[state.MemoryPointer] = 0;
                break;
        }

        return nextInstructionPointer;
    }

    private void BuildJumpMap(List<CowCommand> commands)
    {
        _jumpMap.Clear();
        var loopStack = new Stack<int>();

        for (int i = 0; i < commands.Count; i++)
        {
            if (commands[i] == CowCommand.LoopStart)
            {
                loopStack.Push(i);
            } 
            else if (commands[i] == CowCommand.LoopEnd) 
            {
                if (loopStack.Count > 0)
                {
                    var start = loopStack.Pop();
                    _jumpMap[start] = i;
                    _jumpMap[i] = start;
                }
            }
        }
    }
}