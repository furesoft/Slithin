using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Unary;

namespace Slithin.Scripting.Parsing;

public partial class Parser
{
    private Expr ParseExpression(int parentPrecedence = 0)
    {
        Expr left;
        var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseExpression(unaryOperatorPrecedence);
            left = new UnaryExpression(operatorToken, operand);
        }
        else
        {
            left = ParsePrimary();
        }

        while (true)
        {
            var precedence = Current.Type.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
                break;

            var operatorToken = NextToken();
            var right = ParseExpression(precedence);
            left = new BinaryExpression(left, operatorToken, right);
        }

        return left;
    }

    private Expr ParseGroup()
    {
        Match(TokenType.OpenParen);

        var expr = ParseExpression();

        Match(TokenType.CloseParen);

        return new GroupExpression(expr);
    }

    private Expr ParseNameExpr()
    {
        return new NameExpression(NextToken());
    }

    private Expr ParseNumber()
    {
        return new LiteralNode(double.Parse(NextToken().Text));
    }

    private Expr ParsePrimary()
    {
        if (Current.Type == TokenType.StringLiteral)
        {
            return ParseString();
        }
        else if (Current.Type == TokenType.OpenParen)
        {
            return ParseGroup();
        }
        else if (Current.Type == TokenType.Identifier)
        {
            return ParseNameExpr();
        }
        else if (Current.Type == TokenType.Number)
        {
            return ParseNumber();
        }
        else if (Current.Type == TokenType.At)
        {
            NextToken();

            return new DateExpression(ParseExpression());
        }
        else
        {
            Messages.Add(Message.Error($"Unknown Expression. Expected String, Group, Number or Identifier", Current.Line, Current.Column));
        }

        return new InvalidExpr();
    }

    private Expr ParseString()
    {
        return new LiteralNode(NextToken().Text);
    }
}
