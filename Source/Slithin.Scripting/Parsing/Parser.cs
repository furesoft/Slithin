using Slithin.Scripting.Core;
using Slithin.Scripting.Parsing.AST;

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
                cu.Body.Body.Add(ParseRemember(cu));
            }
            else
            {
                Messages.Add(Message.Error($"Unknown keyword '{keyword.Text}'.", keyword.Line, keyword.Column));
            }
        }

        cu.Messages = Messages;

        return cu;
    }

    private Expr ParseNameExpr()
    {
        return new NameExpression(NextToken());
    }

    private SyntaxNode ParseRemember(CompilationUnit cu)
    {
        throw new NotImplementedException();
    }

    private Expr ParseString()
    {
        return new LiteralNode(NextToken().Text);
    }
}