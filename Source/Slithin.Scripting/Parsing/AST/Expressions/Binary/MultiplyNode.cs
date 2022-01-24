namespace Slithin.Scripting.Parsing.AST.Expressions.Binary
{
    public class MultiplyNode : BinaryExpression
    {
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}