using Slithin.Scripting.Core;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;

namespace Slithin.Scripting.Parsing;

public static class Expression
{
    public static List<OperatorInfo> Operators = new List<OperatorInfo>();

    static Expression()
    {
        //Unary Operators
        Operators.Add(new OperatorInfo(TokenType.Minus, 6, true, false));
        Operators.Add(new OperatorInfo(TokenType.Not, 6, true, false));
        Operators.Add(new OperatorInfo(TokenType.Hours, 7, true, true));
        Operators.Add(new OperatorInfo(TokenType.Minutes, 7, true, true));
        Operators.Add(new OperatorInfo(TokenType.Seconds, 7, true, true));
        Operators.Add(new OperatorInfo(TokenType.DayLiteral, 7, true, true));

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

    public static int GetUnaryOperatorPrecedence(this TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && _.IsUnary).Precedence;
    }

    public static bool IsPostUnary(this TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && _.IsUnary).IsPostUnary;
    }

    public static Expr Parse<TNode, TLexer, TParser>(BaseParser<TNode, TLexer, TParser> parser, int parentPrecedence = 0)
        where TParser : BaseParser<TNode, TLexer, TParser>
        where TLexer : BaseLexer, new()
    {
        Expr left;
        var unaryOperatorPrecedence = parser.Current.Type.GetUnaryOperatorPrecedence();

        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            Token? operatorToken = parser.NextToken();
            Expr? operand = Parse(parser, unaryOperatorPrecedence + 1);

            left = new UnaryExpression(operatorToken, operand, false);
        }
        else
        {
            left = parser.ParsePrimary();

            if (parser.Current.Type.IsPostUnary())
            {
                Token? operatorToken = parser.NextToken();

                left = new UnaryExpression(operatorToken, left, true);
            }
        }

        while (true)
        {
            var precedence = parser.Current.Type.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
                break;

            var operatorToken = parser.NextToken();
            var right = Parse(parser, precedence);

            left = new BinaryExpression(left, operatorToken, right);
        }

        return left;
    }
}
