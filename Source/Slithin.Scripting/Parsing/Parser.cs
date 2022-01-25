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
        while (Peek(1).Type != (TokenType.EOF))
        {
            var keyword = NextToken();

            if (keyword.Type == TokenType.Remember)
            {
                cu.Body.Body.Add(ParseRemember());
            }
            else
            {
                cu.Body.Body.Add(ParseExpression());
                Messages.Add(Message.Error($"Unknown keyword '{keyword.Text}'.", keyword.Line, keyword.Column));
            }
        }

        cu.Messages = Messages;

        return cu;
    }

    private Expr ParseExpression()
    {
        var term = ParseTerm();

        if (Peek(0).Type == TokenType.Plus)
        {
            return new AdditionNode(term, ParseExpression());
        }
        else if (Peek(0).Type == TokenType.Minus)
        {
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

        if (Peek(0).Type == TokenType.Star)
        {
            return new MultiplyNode(unary, ParseTerm());
        }
        else if (Peek(0).Type == TokenType.Slash)
        {
            return new DivideNode(unary, ParseTerm());
        }

        return unary;
    }

    private Expr ParseUnary()
    {
        var primary = ParsePrimary();

        if (Peek(0).Type == TokenType.Not)
        {
            return new AST.Expressions.Unary.NotExpression(primary);
        }
        else if (Peek(0).Type == TokenType.Minus)
        {
            return new AST.Expressions.Unary.NegateExpression(primary);
        }

        return primary;
    }
}