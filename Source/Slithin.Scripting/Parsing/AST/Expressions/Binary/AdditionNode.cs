namespace Slithin.Scripting.Parsing.AST.Expressions.Binary
{
    public class AdditionNode : BinaryExpression
    {
        public AdditionNode(Expr lhs, Expr rhs) : base(lhs, rhs)
        {
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}