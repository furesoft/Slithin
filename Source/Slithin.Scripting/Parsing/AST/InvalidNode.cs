namespace Slithin.Scripting.Parsing.AST;

public class InvalidNode : SyntaxNode
{
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}