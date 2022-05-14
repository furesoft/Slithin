namespace Slithin.Scripting.Parsing.AST.Expressions;

public class BinaryExpression : Expression
{
    public BinaryExpression(Expression lhs, Token operatorToken, Expression rhs)
    {
        Left = lhs;
        OperatorToken = operatorToken;
        Right = rhs;
    }

    public Expression Left { get; }
    public Token OperatorToken { get; }
    public Expression Right { get; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Left} {OperatorToken.Text} {Right}";
    }
}
