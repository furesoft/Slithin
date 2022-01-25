using Slithin.Scripting.Core;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Binary;

namespace Slithin.Scripting.Parsing;

public class Parser : BaseParser<SyntaxNode, Lexer, Parser>
{
    public Parser(SourceDocument document, List<Token> tokens, List<Message> messages) : base(document, tokens, messages)
    {
    }

    protected override SyntaxNode Start()
    {
        var cu = new CompilationUnit();
        while (Current.Type != (TokenType.EOF))
        {
            cu.Body.Body.Add(ParseExpression());

            /*var keyword = NextToken();

            if (keyword.Type == TokenType.Remember)
            {
                cu.Body.Body.Add(ParseRemember());
            }
            else
            {
                Messages.Add(Message.Error($"Unknown keyword '{keyword.Text}'.", keyword.Line, keyword.Column));
            }*/
        }

        cu.Messages = Messages;

        return cu;
    }

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
            Messages.Add(Message.Error($"Unknown Expression. Expected String, Group, CharakterClass or Identifier", Current.Line, Current.Column));
        }

        return new InvalidExpr();
    }

    private SyntaxNode ParseRemember()
    {
        throw new NotImplementedException();
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

        return unary;
    }

    private Expr ParseUnary()
    {
        if (Current.Type == TokenType.Not)
        {
            return new AST.Expressions.Unary.NotExpression(ParsePrimary());
        }
        else if (Current.Type == TokenType.Minus)
        {
            return new AST.Expressions.Unary.NegateExpression(ParsePrimary());
        }

        return ParsePrimary();
    }
}
