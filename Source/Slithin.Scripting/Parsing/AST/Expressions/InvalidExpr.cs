namespace Slithin.Scripting.Parsing.AST.Expressions;

public class InvalidExpr : Expr
{
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}