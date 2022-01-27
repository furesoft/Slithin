namespace Slithin.Scripting.Parsing.AST.Statements;

public class AssignmentStatement : Statement
{
    public AssignmentStatement(Token nameToken, Expr value)
    {
        NameToken = nameToken;
        Value = value;
    }

    public Token NameToken { get; set; }
    public Expr Value { get; set; }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
