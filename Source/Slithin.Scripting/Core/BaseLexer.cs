using Slithin.Scripting.Parsing;
using Slithin.Scripting.Parsing;
namespace Slithin.Scripting.Core;

public abstract class BaseLexer
{
    public List<Message> Messages = new();

    protected int _column = 1;
    protected int _line = 1;
    protected int _position = 0;
    protected string _source = string.Empty;

    public List<Token> Tokenize(string source)
    {
        _source = source;

        var tokens = new List<Token>();

        Token newToken;
        do
        {
            newToken = NextToken();

            tokens.Add(newToken);
        } while (newToken.Type != TokenType.EOF);

        return tokens;
    }

    protected int Advance()
    {
        return _position++;
    }

    protected char Current()
    {
        if (_position >= _source.Length)
        {
            return '\0';
        }

        return _source[_position];
    }

    protected abstract Token NextToken();

    protected char Peek(int offset = 0)
    {
        if (_position + offset >= _source.Length)
        {
            return '\0';
        }

        return _source[_position + offset];
    }

    protected void ReportError()
    {
        Messages.Add(Message.Error($"Unknown Charackter '{Current()}'", _line, _column++));
        Advance();
    }
}
