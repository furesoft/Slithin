namespace Slithin.Scripting.Parsing.AST.Expressions.Binary;

public abstract class BinaryExpression : Expr
{
    public BinaryExpression(Expr lhs, Expr rhs)
    {
        Lhs = lhs;
        Rhs = rhs;
    }

    public Expr Lhs { get; }
    public Expr Rhs { get; }
}
