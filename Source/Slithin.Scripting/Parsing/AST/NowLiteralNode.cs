namespace Slithin.Scripting.Parsing.AST;

public class NowLiteralNode : LiteralNode
{
    public NowLiteralNode(object value) : base(value)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
