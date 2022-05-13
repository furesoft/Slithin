namespace Slithin.Scripting.Parsing.AST.Statements;

public class AssignmentStatement : Statement
{
    public AssignmentStatement(Token nameToken, Expression value)
    {
        NameToken = nameToken;
        Value = value;
    }

    public Token NameToken { get; set; }
    public Expression Value { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
