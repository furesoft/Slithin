using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Binary;
using Slithin.Scripting.Parsing.AST.Expressions.Unary;

namespace Slithin.Scripting.Parsing;

public partial class Parser
{
    private Expr ParseExpression()
    {
        var term = ParseTerm();

        var token = Current;
        if (token.Type == TokenType.Plus)
        {
            NextToken();

            return new AdditionNode(term, ParseExpression());
        }
        else if (token.Type == TokenType.Minus)
        {
            NextToken();

            return new SubtractNode(term, ParseExpression());
        }

        return term;
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

    private Expr ParseTerm()
    {
        var unary = ParseUnary();

        var token = Current;
        if (token.Type == TokenType.Star)
        {
            NextToken();

            return new MultiplyNode(unary, ParseTerm());
        }
        else if (token.Type == TokenType.Slash)
        {
            NextToken();

            return new DivideNode(unary, ParseTerm());
        }
        else if (token.Type == TokenType.Colon)
        {
            NextToken();

            return new TimeNode(unary, ParseTerm());
        }

        return unary;
    }

    private Expr ParseUnary()
    {
        if (Current.Type == TokenType.Not)
        {
            NextToken();

            return new NotExpression(ParsePrimary());
        }
        else if (Current.Type == TokenType.Minus)
        {
            NextToken();

            return new NegateExpression(ParsePrimary());
        }
        else if (Current.Type == TokenType.At)
        {
            NextToken();

            return new DateExpression(ParseExpression());
        }

        return ParsePrimary();
    }
}
