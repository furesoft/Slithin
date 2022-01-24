namespace Slithin.Scripting.Parsing.AST;

public abstract class SyntaxNode
{
    public SyntaxNode(SyntaxNode? parent)
    {
        Parent = parent;
    }

    public SyntaxNode? Parent { get; set; }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
