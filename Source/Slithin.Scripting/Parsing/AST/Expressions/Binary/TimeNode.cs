namespace Slithin.Scripting.Parsing.AST.Expressions.Binary;

public class TimeNode : BinaryExpression
{
    public TimeNode(Expr lhs, Expr rhs) : base(lhs, rhs)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Lhs}:{Rhs}";
    }
}
