namespace Slithin.Scripting.Parsing.AST.Literals;

public class DayLiteralNode : LiteralNode
{
    public DayLiteralNode() : base(null)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
