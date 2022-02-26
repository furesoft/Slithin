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
            "on" or "true" or "enabled" => TokenType.TrueLiteral,
            "off" or "false" or "disabled" => TokenType.FalseLiteral,
            "monday" or "tuesday" or "wednesday" or
            "thursday" or "friday" or "saturday" or
            "sunday" => TokenType.DayOfWeekLiteral,

            "now" => TokenType.NowLiteral,
            "call" or "invoke" => TokenType.Call,
            "set" => TokenType.Set,
            "change" => TokenType.Set,
            "to" => TokenType.To,
            "with" => TokenType.With,
            "and" => TokenType.And,
            "every" => TokenType.Every,

            "minute" or "minutes" => TokenType.Minutes,
            "second" or "seconds" => TokenType.Seconds,
            "hour" or "hours" => TokenType.Hours,
            "day" or "days" => TokenType.DayLiteral,

            _ => TokenType.Identifier,
        };
    }
}
