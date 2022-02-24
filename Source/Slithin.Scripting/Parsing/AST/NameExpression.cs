namespace Slithin.Scripting.Parsing.AST;

public class NameExpression : Expression
{
    public NameExpression(string name, int line, int column)
    {
        Name = name;
        Line = line;
        Column = column;
    }

    public int Column { get; set; }
    public int Line { get; set; }
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
