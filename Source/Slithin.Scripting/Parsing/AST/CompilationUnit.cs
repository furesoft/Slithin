using Slithin.Scripting.Parsing.AST;
namespace Slithin.Scripting.Parsing.AST;

public class CompilationUnit : SyntaxNode
{
    public CompilationUnit() : base(null)
    {
    }

    public Block Body { get; set; } = new Block();
    public List<Message> Messages { get; set; } = new List<Message>();

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
