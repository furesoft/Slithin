namespace Slithin.Scripting.Parsing.AST.Expressions;

public class UnaryExpression : Expr
{
    public UnaryExpression(Token operatorToken, Expr expression)
    {
        Expression = expression;
        OperatorToken = operatorToken;
    }

    public Expr Expression { get; set; }
    public Token OperatorToken { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{OperatorToken.Text}{Expression}";
    }
}
