namespace Slithin.Scripting.Parsing.AST.Expressions;

public class GroupExpression : Expression
{
    public GroupExpression(Expression inner)
    {
        Inner = inner;
    }

    public Expression Inner { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
