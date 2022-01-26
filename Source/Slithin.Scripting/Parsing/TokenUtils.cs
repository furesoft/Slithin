namespace Slithin.Scripting.Parsing;

public static class TokenUtils
{
    public static int GetBinaryOperatorPrecedence(this TokenType kind)
    {
        switch (kind)
        {
            case TokenType.Star:
            case TokenType.Slash:
            case TokenType.Colon:
                return 5;

            case TokenType.Plus:
            case TokenType.Minus:
                return 4;

            default:
                return 0;
        }
    }

    public static TokenType GetTokenType(string name)
    {
        return name switch
        {
            "as" => TokenType.As,
            "at" => TokenType.At,
            "not" => TokenType.Not,
            "negate" => TokenType.Minus,
            "remember" => TokenType.Remember,
            _ => TokenType.Identifier,
        };
    }

    public static int GetUnaryOperatorPrecedence(this TokenType kind)
    {
        switch (kind)
        {
            case TokenType.Minus:
            case TokenType.Not:
            case TokenType.At:
                return 6;

            default:
                return 0;
        }
    }
}
