namespace Slithin.Scripting.Parsing.AST.Expressions.Unary;

public class DateExpression : UnaryExpression
{
    public DateExpression(Expr expression) : base(expression)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
