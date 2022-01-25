namespace Slithin.Scripting.Parsing.AST.Expressions.Unary;

public abstract class UnaryExpression : Expr
{
    protected UnaryExpression(Expr expression)
    {
        Expression = expression;
    }

    public Expr Expression { get; set; }
}