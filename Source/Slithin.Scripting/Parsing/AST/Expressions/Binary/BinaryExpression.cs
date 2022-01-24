using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST.Expressions.Binary;

public abstract class BinaryExpression : Expr
{
    protected BinaryExpression(Expr lhs, string symbol, Expr rhs, SyntaxNode? parent = null) : base(parent)
    {
        Lhs = lhs;
        Symbol = symbol;
        Rhs = rhs;
    }

    public Expr Lhs { get; }
    public Expr Rhs { get; }
    public string Symbol { get; set; }

    public override string ToString()
    {
        return $"({Lhs} {Symbol} {Rhs})";
    }
}
