namespace Slithin.Scripting.Parsing.AST.Expressions;

public class CallExpr : Expression
{
    public CallExpr(Expression identifiers, Expression? interval, Block arguments)
    {
        Identifiers = identifiers;
        Arguments = arguments;
        Interval = interval;
    }

    public Block Arguments { get; }
    public Expression Identifiers { get; }

    public Expression? Interval { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
