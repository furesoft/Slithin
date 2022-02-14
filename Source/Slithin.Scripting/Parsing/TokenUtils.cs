namespace Slithin.Scripting.Parsing;

public static class TokenUtils
{
    public static List<OperatorInfo> Operators = new List<OperatorInfo>();

    static TokenUtils()
    {
        //Unary Operators
        Operators.Add(new OperatorInfo(TokenType.Minus, 6, true, false));
        Operators.Add(new OperatorInfo(TokenType.Not, 6, true, false));
        Operators.Add(new OperatorInfo(TokenType.Hours, 7, true, true));
        Operators.Add(new OperatorInfo(TokenType.Minutes, 7, true, true));
        Operators.Add(new OperatorInfo(TokenType.Seconds, 7, true, true));

        //Binary Operators
        Operators.Add(new OperatorInfo(TokenType.Star, 5, false, false));
        Operators.Add(new OperatorInfo(TokenType.Slash, 5, false, false));

        Operators.Add(new OperatorInfo(TokenType.Plus, 4, false, false));
        Operators.Add(new OperatorInfo(TokenType.Minus, 4, false, false));

        Operators.Add(new OperatorInfo(TokenType.Comma, 2, false, false));
    }

    public static int GetBinaryOperatorPrecedence(this TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && !_.IsUnary).Precedence;
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
            "on" or "true" or "enabled" => TokenType.TrueLiteral,
            "off" or "false" or "disabled" => TokenType.FalseLiteral,
            "monday" or "tuesday" or "wednesday" or
            "thursday" or "friday" or "saturday" or
            "sunday" => TokenType.DayOfWeekLiteral,
            "day" => TokenType.DayLiteral,
            "now" => TokenType.NowLiteral,
            "call" or "invoke" => TokenType.Call,
            "set" => TokenType.Set,
            "change" => TokenType.Set,
            "to" => TokenType.To,
            "with" => TokenType.With,
            "and" => TokenType.And,
            "every" => TokenType.Every,

            "minutes" => TokenType.Minutes,
            "seconds" => TokenType.Seconds,
            "hours" => TokenType.Hours,

            _ => TokenType.Identifier,
        };
    }

    public static int GetUnaryOperatorPrecedence(this TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && _.IsUnary).Precedence;
    }

    public static bool IsPostUnary(this TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && _.IsUnary).IsPostUnary;
    }
}
