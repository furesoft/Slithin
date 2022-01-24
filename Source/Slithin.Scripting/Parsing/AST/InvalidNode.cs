using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST;

public class InvalidNode : SyntaxNode
{
    public InvalidNode(SyntaxNode? parent = null) : base(parent)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
