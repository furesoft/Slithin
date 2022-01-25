namespace Slithin.Scripting.Parsing.AST.Expressions.Binary;

public class DivideNode : BinaryExpression
{
    public DivideNode(Expr lhs, Expr rhs) : base(lhs, rhs)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
