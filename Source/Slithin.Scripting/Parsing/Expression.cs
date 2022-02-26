using Slithin.Scripting.Core;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;

namespace Slithin.Scripting.Parsing;

public class Expression : SyntaxNode
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

    public static Expression Parse<TNode, TLexer, TParser>(BaseParser<TNode, TLexer, TParser> parser, int parentPrecedence = 0)
        where TParser : BaseParser<TNode, TLexer, TParser>
        where TLexer : BaseLexer, new()
    {
        Expression left;
        var unaryOperatorPrecedence = GetUnaryOperatorPrecedence(parser.Current.Type);

        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            Token? operatorToken = parser.NextToken();
            Expression? operand = Parse(parser, unaryOperatorPrecedence + 1);

            left = new UnaryExpression(operatorToken, operand, false);
        }
        else
        {
            left = parser.ParsePrimary();

            if (IsPostUnary(parser.Current.Type))
            {
                Token? operatorToken = parser.NextToken();

                left = new UnaryExpression(operatorToken, left, true);
            }
        }

        while (true)
        {
            var precedence = GetBinaryOperatorPrecedence(parser.Current.Type);
            if (precedence == 0 || precedence <= parentPrecedence)
                break;

            var operatorToken = parser.NextToken();
            var right = Parse(parser, precedence);

            left = new BinaryExpression(left, operatorToken, right);
        }

        return left;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    private static int GetBinaryOperatorPrecedence(TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && !_.IsUnary).Precedence;
    }

    private static int GetUnaryOperatorPrecedence(TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && _.IsUnary).Precedence;
    }

    private static bool IsPostUnary(TokenType kind)
    {
        return Operators.FirstOrDefault(_ => _.Token == kind && _.IsUnary).IsPostUnary;
    }
}
