namespace Slithin.Scripting.Parsing;

public static class TokenUtils
{
    public static TokenType GetTokenType(string name)
    {
        return name switch
        {
            "as" => TokenType.As,
            "divide" => TokenType.Divide,
            "multiply" => TokenType.Multiply,
            "remember" => TokenType.Remember,
            _ => TokenType.Identifier,
        };
    }
}
