namespace Pascal.Ast;

using Pascal.Token;

public class Node {};

public class Number : Node
{
    public Token Token { get; }

    public Number(Token token)
    {
        this.Token = token;
    }

    public override string ToString()
    {
        return $"{GetType().Name}({Token}')";
    }
}

public class BinOp : Node
{
    public Node Left { get; }
    public Token Op { get; }
    public Node Right { get; }

    public BinOp(Node left, Token op, Node right)
    {
        this.Left = left;
        this.Op = op;
        this.Right = right;
    }

    public override string ToString()
    {
        return $"BinOp{Op.Value}({Left}, {Right})";
    }
}

public class Compound : Node
{
    public List<Node> Children { get; } = new List<Node>();
    public override string ToString()
    {
        return $"Compound({string.Join(", ", Children)})";
    }
}

public class Var : Node
{
    public Token Token { get; }
    public string Name;

    public Var(Token token)
    {
        this.Token = token;
        this.Name = token.Value;
    }

    public override string ToString()
    {
        return $"Var(Name={Name})";
    }
}

public class Assign : Node
{
    public Var Left { get; }
    public Node Right { get; }

    public Assign(Var left, Node right)
    {
        this.Left = left;
        this.Right = right;
    }

    public override string ToString()
    {
        return $"Assign({Left} := {Right})";
    }
}

public class NoOp : Node
{
    public override string ToString() 
    {
        return "NoOp";
    }
}

public class ProgramNode : Node
{
    public string Name { get; }
    public Node Block { get; }

    public ProgramNode(Node block)
    {
        Name = "Default";
        Block = block;
    }
    public override string ToString() 
    { 
        return $"Program(Name={Name}, {Block})";
    }
}