namespace Slithin.Scripting.Parsing.AST.Expressions;

public class BinaryExpression : Expr
{
    public BinaryExpression(Expr lhs, Token operatorToken, Expr rhs)
    {
        Lhs = lhs;
        OperatorToken = operatorToken;
        Rhs = rhs;
    }

    public Expr Lhs { get; }
    public Token OperatorToken { get; }
    public Expr Rhs { get; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Lhs}{OperatorToken.Text}{Rhs}";
    }
}
