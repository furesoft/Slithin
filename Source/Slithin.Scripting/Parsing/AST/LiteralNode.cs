namespace Slithin.Scripting.Parsing.AST;

public class LiteralNode : Expr
{
    public LiteralNode(object value)
    {
        Value = value;
    }

    public object Value { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Value}";
    }
}
