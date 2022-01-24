using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST.Expressions;

public class InvalidExpr : Expr
{
    public InvalidExpr(SyntaxNode? parent = null) : base(parent)
    {
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
