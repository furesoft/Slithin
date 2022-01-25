namespace Slithin.Scripting.Parsing;

public static class TokenUtils
{
    public static TokenType GetTokenType(string name)
    {
        return name switch
        {
            "as" => TokenType.As,
            "not" => TokenType.Not,
            "negate" => TokenType.Minus,
            "remember" => TokenType.Remember,
            _ => TokenType.Identifier,
        };
    }
}