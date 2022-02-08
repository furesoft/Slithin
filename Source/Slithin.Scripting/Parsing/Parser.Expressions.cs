using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Literals;

namespace Slithin.Scripting.Parsing;

public partial class Parser
{
    private Expr Invalid(string message)
    {
        Messages.Add(Message.Error(message, Current.Line, Current.Column));

        return new InvalidExpr();
    }

    private Expr ParseBooleanLiteral(bool value)
    {
        NextToken();

        return new LiteralNode(value);
    }

    private Expr ParseDayLiteral()
    {
        NextToken();

        return new DayLiteralNode();
    }

    private Expr ParseDayOfWeekLiteral()
    {
        var token = NextToken();

        return new LiteralNode(Enum.Parse<DayOfWeek>(token.Text.Substring(0, token.Text.Length - 1), true));
    }

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

    private Expr ParseIdentifierList()
    {
        var identifiers = new List<string>();

        Token token;
        int line = -1, column = -1;

        do
        {
            token = NextToken();

            if (line == -1 && column == -1)
            {
                line = token.Line;
                column = token.Column;
            }

            identifiers.Add(token.Text);
        } while (Current.Type == TokenType.Identifier);

        return new NameExpression(string.Join(' ', identifiers), line, column);
    }

    private Expr ParseIdentifierListOrCall()
    {
        var identifiers = ParseIdentifierList();

        if (Current.Type == TokenType.With)
        {
            NextToken();

            Expr interval = null;

            var arguments = new Block();

            bool hasArgumentLeft = false;
            do
            {
                hasArgumentLeft = false;

                arguments.Body.Add(ParseExpression());

                if (Current.Type == TokenType.And)
                {
                    NextToken();
                    hasArgumentLeft = true;
                }
            } while (hasArgumentLeft);

            if (Current.Type == TokenType.Every)
            {
                NextToken();

                interval = ParseExpression();
            }

            return new CallExpr(identifiers, interval, arguments);
        }

        return identifiers;
    }

    private Expr ParseNowLiteral()
    {
        NextToken();

        return new NowLiteralNode(null);
    }

    private Expr ParseNumber()
    {
        return new LiteralNode(double.Parse(NextToken().Text));
    }

    private Expr ParsePrimary()
    {
        return Current.Type switch
        {
            TokenType.StringLiteral => ParseString(),
            TokenType.OpenParen => ParseGroup(),
            TokenType.Identifier => ParseIdentifierListOrCall(),
            TokenType.Number => ParseNumber(),
            TokenType.At => new UnaryExpression(NextToken(), ParseExpression()),
            TokenType.TrueLiteral => ParseBooleanLiteral(true),
            TokenType.FalseLiteral => ParseBooleanLiteral(false),
            TokenType.DayLiteral => ParseDayLiteral(),
            TokenType.DayOfWeekLiteral => ParseDayOfWeekLiteral(),
            TokenType.NowLiteral => ParseNowLiteral(),
            _ => Invalid("Unknown Expression. Expected String, Group, Number, Boolean or Identifier"),
        };
    }

    private Expr ParseString()
    {
        return new LiteralNode(NextToken().Text);
    }
}
