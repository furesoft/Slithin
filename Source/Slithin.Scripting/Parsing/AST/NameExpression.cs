namespace Slithin.Scripting.Parsing.AST;

public class NameExpression : Expr
{
    public NameExpression(Token nameToken)
    {
        Name = nameToken.Text;
        NameToken = nameToken;
    }

    public string Name { get; set; }
    public Token NameToken { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}