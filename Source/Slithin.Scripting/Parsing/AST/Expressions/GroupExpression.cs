namespace Slithin.Scripting.Parsing.AST.Expressions;

public class GroupExpression : Expr
{
    public GroupExpression(Expr inner)
    {
        Inner = inner;
    }

    public Expr Inner { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
