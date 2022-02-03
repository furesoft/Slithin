namespace Slithin.Scripting.Parsing.AST;

public class NameExpression : Expr
{
    public NameExpression(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}
