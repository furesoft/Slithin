namespace Slithin.Scripting.Parsing.AST.Expressions.Binary
{
    public class DivideNode : BinaryExpression
    {
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}