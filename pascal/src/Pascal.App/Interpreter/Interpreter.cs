namespace Pascal.Interpreter;

using System.Data;
using Pascal.Ast;
using Pascal.Lexer;
using Pascal.Parser;

public class NodeVizitor
{
    public void Vizit() {}
}

public class Interpreter
{
    private Lexer _lexer; 
    private Parser _parser;
    private Dictionary<string, float> _symbolTable = new();

    public Interpreter()
    {
        _lexer = new Lexer();
        _parser = new Parser(_lexer);
    }

    public Dictionary<string, float> GetSymbolTable()
    {
        return _symbolTable;
    }

    public float Vizit(Node node)
    {
        return node switch
        {
            ProgramNode n => VizitProgram(n),
            Compound n => VizitCompound(n),
            Assign n => VizitAssign(n),
            Var n => VizitVar(n),
            NoOp => 0,
            BinOp n => VizitBinOp(n),
            Number n => VizitNumber(n),
            _ => throw new SyntaxErrorException("Invalid node type")
        };
    }

    public float VizitProgram(ProgramNode node)
    {
        return Vizit(node.Block);
    }

    public float VizitCompound(Compound node)
    {
        foreach (var child in node.Children)
        {
            Vizit(child);
        }
        return 0;
    }

    public float VizitAssign(Assign node)
    {
        string varName = node.Left.Name;
        float varValue = Vizit(node.Right);
        _symbolTable[varName] = varValue;
        return varValue;
    }

    public float VizitVar(Var node)
    {
        string varName = node.Name;
        if (_symbolTable.TryGetValue(varName, out float value))
        {
            return value;
        }
        throw new KeyNotFoundException($"Variable '{varName}' is not defined.");
    }

    public float VizitNumber(Number node)
    {
        return float.Parse(node.Token.Value);
    }

    public float VizitBinOp(BinOp node)
    {
        switch (node.Op.Value)
        {
            case "+":
                return Vizit(node.Left) + Vizit(node.Right);
            case "-":
                return Vizit(node.Left) - Vizit(node.Right);
            case "*":
                return Vizit(node.Left) * Vizit(node.Right);
            case "/":
                float rightValue = Vizit(node.Right);
                if (rightValue == 0)
                {
                    throw new DivideByZeroException("An attempt to divide by zero");
                }
                return Vizit(node.Left) / Vizit(node.Right);
            default:
                throw new InvalidOperationException($"Unsupported operator: {node.Op.Value}");
        }
    }

    public float Eval(string text)
    {
        Node tree = _parser.Parse(text);
        return Vizit(tree);
    }
}