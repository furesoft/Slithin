namespace Slithin.Scripting.Parsing;

public static class TokenUtils
{
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
}
