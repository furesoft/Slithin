using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST;

public abstract class Expr : SyntaxNode
{
    protected Expr(SyntaxNode? parent) : base(parent)
    {
    }
}
