namespace Slithin.Scripting.Parsing.AST.Statements;

public class RememberStatement : Statement
{
    public RememberStatement(string name, Expr value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; }
    public Expr Value { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
