namespace Slithin.Scripting.Parsing.AST.Expressions.Unary;

public abstract class UnaryExpression : Expr
{
    public Expr Expression { get; set; }
}