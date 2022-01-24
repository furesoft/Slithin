using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST.Expressions.Unary;

public abstract class UnaryExpression : Expr
{
    protected UnaryExpression(string symbol, Expr expression, SyntaxNode? parent = null) : base(parent)
    {
        Symbol = symbol;
        Expression = expression;
    }

    public Expr Expression { get; set; }
    public string Symbol { get; set; }

    public override string ToString()
    {
        return $"({Expression}){Symbol}";
    }
}
