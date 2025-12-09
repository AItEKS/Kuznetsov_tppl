namespace Pascal.Lexer;

using System.Data;
using System.Text;

using Pascal.Token;

public class Lexer
{
    private int _pos = 0;
    private char? _currentChar;
    private string _text = "";

    private Dictionary<string, Token> _reservedKeywords;

    public Lexer()
    {
        _reservedKeywords = new Dictionary<string, Token>(StringComparer.OrdinalIgnoreCase)
        {
            { "BEGIN", new Token(TokenType.BEGIN, "BEGIN") },
            { "END", new Token(TokenType.END, "END") }
        };
    }

    public Dictionary<string, Token> GetReservedKeywords() => _reservedKeywords;
    
    private void Forward()
    {
        _pos += 1;
        if (_pos > _text.Length - 1)
        {
            _currentChar = null;
        } 
        else
        {
            _currentChar = _text[_pos];    
        }
    }

    private void Skip()
    {
        while (_currentChar is not null && char.IsWhiteSpace(_currentChar.Value))
        {
            Forward();
        }
    }

    private string Number()
    {
        var builder = new StringBuilder();
        
        while (_currentChar is not null && char.IsDigit(_currentChar.Value))
        {
            builder.Append(_currentChar.Value);
            Forward();
        }
        
        return builder.ToString();
    }

    private Token Identifier()
    {
        var builder = new StringBuilder();
        while (_currentChar is not null && char.IsLetterOrDigit(_currentChar.Value))
        {
            builder.Append(_currentChar.Value);
            Forward();
        }

        string result = builder.ToString();
        if (_reservedKeywords.TryGetValue(result, out var token))
        {
            return token;
        }
        
        return new Token(TokenType.ID, result);
    }

    private char? AheadToken()
    {
        int peekPos = _pos + 1;
        if (peekPos > _text.Length - 1)
        {
            return null;
        }
        return _text[peekPos];
    }

    public Token NextToken()
    {
        while (_currentChar is not null)
        {
            char ch = _currentChar.Value;

            if (char.IsWhiteSpace(_currentChar.Value))
            {
                Skip();
                continue;
            }

            if (char.IsLetter(ch))
            {
                return Identifier();
            }

            if (char.IsDigit(ch)) 
            {
                return new Token(TokenType.Number, Number());
            }

            if (ch == ':' && AheadToken() == '=')
            {
                Forward(); // ':'
                Forward(); // '='
                return new Token(TokenType.ASSIGN, ":=");
            }
            
            switch(ch)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    Forward();
                    return new Token(TokenType.Operator, ch.ToString());
                case '(':
                    Forward();
                    return new Token(TokenType.LParen, ch.ToString());
                case ')':
                    Forward();
                    return new Token(TokenType.RParen, ch.ToString());
                case ';':
                    Forward();
                    return new Token(TokenType.SEMI, ch.ToString());
                case '.':
                    Forward();
                    return new Token(TokenType.DOT, ch.ToString());
                default:
                    throw new SyntaxErrorException($"Unknown or invalid character: '{ch}'");
            }
        }

        return new Token(TokenType.EOL, "");
    }

    public void SetText(string expr)
    {
        _text = expr;
        _pos = 0;
        _currentChar = _text[_pos];
    }
}