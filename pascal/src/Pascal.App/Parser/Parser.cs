namespace Pascal.Parser;

using Pascal.Token;
using Pascal.Lexer;
using Pascal.Ast;

using System.Data;

public class Parser
{
    public Lexer Lexer { get; }
    private Token? _currentToken = null;

    public Parser(Lexer lexer)
    {
        this.Lexer = lexer;
    }

    private void CheckTokenType(TokenType type)
    {
        if (_currentToken is not null && _currentToken.Type == type)
        {
            _currentToken = Lexer.NextToken();
        }
        else
        {
            throw new SyntaxErrorException("Invalid token type");
        }
    }

    private Node Factor()
    {
        Token? token = _currentToken;
        if (token is not null && token.Type == TokenType.Number)
        {
            CheckTokenType(TokenType.Number);
            return new Number(token);
        }

        if (token is not null && token.Type == TokenType.LParen)
        {
            CheckTokenType(TokenType.LParen);
            Node result = Expr();
            CheckTokenType(TokenType.RParen);
            return result;
        }

        if (token is not null && token.Type == TokenType.ID)
        {
            return Variable();
        }

        throw new SyntaxErrorException("Invalid factor");
    }

    private Node Term()
    {
        Node result = Factor();

        while (_currentToken is not null && (_currentToken.Value == "*" || _currentToken.Value == "/"))
        {
            Token token = _currentToken;
            CheckTokenType(TokenType.Operator);
            result = new BinOp(result, token, Factor());
        }

        return result;
    }

    private Node Expr()
    {
        Node result = Term();

        while (_currentToken is not null && (_currentToken.Value == "+" || _currentToken.Value == "-"))
        {
            Token token = _currentToken;
            CheckTokenType(TokenType.Operator);
            result = new BinOp(result, token, Term());
        }

        return result;
    }

    private Var Variable()
    {
        var node = new Var(_currentToken!);
        CheckTokenType(TokenType.ID);
        return node;
    }

    private Node AssignmentStatement()
    {
        Var left = Variable();
        CheckTokenType(TokenType.ASSIGN);
        Node right = Expr();
        return new Assign(left, right);
    }

    private Node Statement()
    {
        if (_currentToken?.Type == TokenType.BEGIN)
        {
            return CompoundStatement();
        }
        if (_currentToken?.Type == TokenType.ID)
        {
            return AssignmentStatement();
        }
        
        return new NoOp();   
    }

    private List<Node> StatementList()
    {
        Node node = Statement();
        var results = new List<Node> { node };

        while (_currentToken?.Type == TokenType.SEMI)
        {
            CheckTokenType(TokenType.SEMI);
            results.Add(Statement());
        }

        if (_currentToken?.Type == TokenType.ID)
        {
            throw new SyntaxErrorException("Invalid syntax: missing semicolon?");
        }

        return results;
    }

    private Node CompoundStatement()
    {
        CheckTokenType(TokenType.BEGIN);
        List<Node> nodes = StatementList();
        CheckTokenType(TokenType.END);

        var root = new Compound();
        root.Children.AddRange(nodes);
        return root;
    }

    private Node Program()
    {
        Node node = CompoundStatement();
        CheckTokenType(TokenType.DOT);
        return new ProgramNode(node);
    }

    public Node Parse(string text)
    {
        Lexer.SetText(text);
        _currentToken = Lexer.NextToken();

        Node tree = Program();
    
        if (_currentToken.Type != TokenType.EOL)
        {
            throw new SyntaxErrorException("Extra code");
        }

        return tree;
    }
}