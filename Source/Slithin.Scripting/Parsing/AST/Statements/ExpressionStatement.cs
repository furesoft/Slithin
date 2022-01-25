namespace Slithin.Scripting.Parsing.AST.Statements;

public class ExpressionStatement : Statement
{
    public ExpressionStatement(Expr expression)
    {
        Expression = expression;
    }

    public Expr Expression { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
