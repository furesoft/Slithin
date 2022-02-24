namespace Slithin.Scripting.Parsing.AST.Expressions;

public class UnaryExpression : Expression
{
    public UnaryExpression(Token operatorToken, Expression expression, bool isPostUnary)
    {
        Expression = expression;
        IsPostUnary = isPostUnary;
        OperatorToken = operatorToken;
    }

    public Expression Expression { get; set; }
    public bool IsPostUnary { get; }
    public Token OperatorToken { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        if (!IsPostUnary)
        {
            return $"{OperatorToken.Text} {Expression}";
        }
        else
        {
            return $"{Expression} {OperatorToken.Text}";
        }
    }
}
