using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST.Literals;

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
