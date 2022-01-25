namespace Slithin.Scripting.Parsing.AST.Expressions.Unary;

public class NegateExpression : UnaryExpression
{
    public NegateExpression(Expr expression) : base(expression)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}