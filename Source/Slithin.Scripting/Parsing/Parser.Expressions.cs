using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Literals;

namespace Slithin.Scripting.Parsing;

public partial class Parser
{
    internal override Expr ParsePrimary()
    {
        return Current.Type switch
        {
            TokenType.StringLiteral => ParseString(),
            TokenType.OpenParen => ParseGroup(),
            TokenType.Identifier => ParseIdentifierListOrCall(),
            TokenType.Number => ParseNumber(),
            TokenType.At => new UnaryExpression(NextToken(), Expression.Parse(this), false),
            TokenType.TrueLiteral => ParseBooleanLiteral(true),
            TokenType.FalseLiteral => ParseBooleanLiteral(false),
            TokenType.DayLiteral => ParseDayLiteral(),
            TokenType.DayOfWeekLiteral => ParseDayOfWeekLiteral(),
            TokenType.NowLiteral => ParseNowLiteral(),
            TokenType.Hours or TokenType.Minutes or TokenType.Seconds => ParseTimeLiteral(),
            _ => Invalid("Unknown Expression. Expected String, Group, Number, Boolean or Identifier"),
        };
    }

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

    private Expr ParseGroup()
    {
        Match(TokenType.OpenParen);

        var expr = Expression.Parse(this);

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

    private Expr ParseNowLiteral()
    {
        NextToken();

        return new NowLiteralNode(null);
    }

    private Expr ParseNumber()
    {
        return new LiteralNode(double.Parse(NextToken().Text));
    }

    private Expr ParseString()
    {
        return new LiteralNode(NextToken().Text);
    }

    private Expr ParseTimeLiteral()
    {
        return new UnaryExpression(Current, null, false);
    }
}
