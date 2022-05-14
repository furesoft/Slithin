namespace Slithin.Scripting.Parsing;

public record struct OperatorInfo(TokenType Token, int Precedence, bool IsUnary, bool IsPostUnary);
