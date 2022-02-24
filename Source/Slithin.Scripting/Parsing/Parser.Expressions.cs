using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Literals;

namespace Slithin.Scripting.Parsing;

public partial class Parser
{
    internal override Expression ParsePrimary()
    {
        return Current.Type switch
        {
            TokenType.StringLiteral => ParseString(),
            TokenType.OpenParen => ParseGroup(),
            TokenType.Identifier => ParseIdentifierListOrCall(),
            TokenType.Number => ParseNumber(),
            TokenType.At => ParseAt(),
            TokenType.TrueLiteral => ParseBooleanLiteral(true),
            TokenType.FalseLiteral => ParseBooleanLiteral(false),
            TokenType.DayLiteral => ParseDayLiteral(),
            TokenType.DayOfWeekLiteral => ParseDayOfWeekLiteral(),
            TokenType.NowLiteral => ParseNowLiteral(),
            TokenType.Hours or TokenType.Minutes or TokenType.Seconds => ParseTimeLiteral(),
            _ => Invalid("Unknown Expression. Expected String, Group, Number, Boolean, Day, DayOfWeek, Now, Time or Identifier"),
        };
    }

    private Expression Invalid(string message)
    {
        Messages.Add(Message.Error(message, Current.Line, Current.Column));

        return new InvalidExpr();
    }

    private UnaryExpression ParseAt()
    {
        return new UnaryExpression(NextToken(), Expression.Parse(this), false);
    }

    private Expression ParseBooleanLiteral(bool value)
    {
        NextToken();

        return new LiteralNode(value);
    }

    private Expression ParseDayLiteral()
    {
        NextToken();

        return new DayLiteralNode();
    }

    private Expression ParseDayOfWeekLiteral()
    {
        var token = NextToken();

        return new LiteralNode(Enum.Parse<DayOfWeek>(token.Text.Substring(0, token.Text.Length - 1), true));
    }

    private Expression ParseGroup()
    {
        Match(TokenType.OpenParen);

        var expr = Expression.Parse(this);

        Match(TokenType.CloseParen);

        return new GroupExpression(expr);
    }

    private Expression ParseIdentifierList()
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

    private Expression ParseIdentifierListOrCall()
    {
        var identifiers = ParseIdentifierList();

        if (Current.Type == TokenType.With)
        {
            NextToken();

            Expression? interval = null;

            var arguments = new Block();

            bool hasArgumentLeft;
            do
            {
                hasArgumentLeft = false;

                arguments.Body.Add(Expression.Parse(this));

                if (Current.Type == TokenType.And)
                {
                    NextToken();
                    hasArgumentLeft = true;
                }
            } while (hasArgumentLeft);

            if (Current.Type == TokenType.Every)
            {
                NextToken();

                interval = Expression.Parse(this);
            }

            return new CallExpr(identifiers, interval, arguments);
        }

        return identifiers;
    }

    private Expression ParseNowLiteral()
    {
        NextToken();

        return new NowLiteralNode(null);
    }

    private Expression ParseNumber()
    {
        return new LiteralNode(double.Parse(NextToken().Text));
    }

    private Expression ParseString()
    {
        return new LiteralNode(NextToken().Text);
    }

    private Expression ParseTimeLiteral()
    {
        return new UnaryExpression(Current, null, false);
    }
}
