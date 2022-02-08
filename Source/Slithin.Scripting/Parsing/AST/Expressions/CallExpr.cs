namespace Slithin.Scripting.Parsing.AST.Expressions;

public class CallExpr : Expr
{
    public CallExpr(Expr identifiers, Expr interval, Block arguments)
    {
        Identifiers = identifiers;
        Arguments = arguments;
        Interval = interval;
    }

    public Block Arguments { get; }
    public Expr Identifiers { get; }

    public Expr Interval { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
