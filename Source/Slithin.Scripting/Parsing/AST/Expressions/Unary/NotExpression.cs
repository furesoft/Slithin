namespace Slithin.Scripting.Parsing.AST.Expressions.Unary;

public class NotExpression : UnaryExpression
{
    public NotExpression(Expr expression) : base(expression)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}