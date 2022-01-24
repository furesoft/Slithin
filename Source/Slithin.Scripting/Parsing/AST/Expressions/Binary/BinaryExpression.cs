namespace Slithin.Scripting.Parsing.AST.Expressions.Binary;

public abstract class BinaryExpression : Expr
{
    public BinaryExpression()
    {
    }

    public Expr Lhs { get; }
    public Expr Rhs { get; }
}