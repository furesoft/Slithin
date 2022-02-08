namespace Slithin.Scripting.Parsing.AST.Expressions;

public class CallExpr : Expr
{
    public CallExpr(Expr identifiers, Block arguments)
    {
        Identifiers = identifiers;
        Arguments = arguments;
    }

    public Block Arguments { get; }
    public Expr Identifiers { get; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
