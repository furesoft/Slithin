using Slithin.Scripting.Core;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Statements;

namespace Slithin.Scripting.Parsing;

public partial class Parser : BaseParser<SyntaxNode, Lexer, Parser>
{
    public Parser(SourceDocument document, List<Token> tokens, List<Message> messages) : base(document, tokens, messages)
    {
    }

    protected override SyntaxNode Start()
    {
        var cu = new CompilationUnit();
        while (Current.Type != (TokenType.EOF))
        {
            var keyword = Current;

            if (keyword.Type == TokenType.Remember)
            {
                cu.Body.Body.Add(ParseRemember());
            }
            else
            {
                cu.Body.Body.Add(ParseExpressionStatement());
            }
        }

        cu.Messages = Messages;

        return cu;
    }

    private SyntaxNode ParseExpressionStatement()
    {
        var expr = ParseExpression();

        Match(TokenType.Dot);

        return new ExpressionStatement(expr);
    }

    private SyntaxNode ParseRemember()
    {
        NextToken();

        var value = ParseExpression();

        Match(TokenType.As);

        var name = Match(TokenType.Identifier);

        Match(TokenType.Dot);

        return new RememberStatement(name, value);
    }
}
