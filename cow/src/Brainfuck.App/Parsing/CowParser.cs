namespace Brainfuck.Parsing;

public class CowParser
{ 
    private static readonly Dictionary<string, CowCommand> CommandMap = new()
    {
        {"MoO", CowCommand.Increment},
        {"MOo", CowCommand.Decrement},
        {"moO", CowCommand.MoveNext},
        {"mOo", CowCommand.MovePrevious},
        {"MOO", CowCommand.LoopStart},
        {"moo", CowCommand.LoopEnd},
        {"OOM", CowCommand.Output},
        {"oom", CowCommand.Input},
        {"mOO", CowCommand.Execute},
        {"Moo", CowCommand.InOrOut},
        {"OOO", CowCommand.Reset},
    };

    public List<CowCommand> Parse(string sourceCode)
    {
        var commands = new List<CowCommand>();
        var tokens = sourceCode.Split(new [] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            if (CommandMap.TryGetValue(token, out var command))
            {
                commands.Add(command);
            }
        }

        return commands;
    }
}