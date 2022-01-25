using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Binary;
using Slithin.Scripting.Parsing.AST.Expressions.Unary;

namespace Slithin.Scripting.Execution
{
    public class Interpreter : IVisitor<object>
    {
        public object Visit(InvalidNode invalidNode)
        {
            throw new NotImplementedException();
        }

        public object Visit(LiteralNode literal)
        {
            return literal.Value;
        }

        public object Visit(CompilationUnit compilationUnit)
        {
            return compilationUnit.Body.Accept(this);
        }

        public object Visit(Block block)
        {
            foreach (var node in block.Body)
            {
                node.Accept(this);
            }

            return 0;
        }

        public object Visit(InvalidExpr invalidExpr)
        {
            throw new NotImplementedException();
        }

        public object Visit(NameExpression nameExpression)
        {
            throw new NotImplementedException();
        }

        public object Visit(GroupExpression groupExpression)
        {
            throw new NotImplementedException();
        }

        public object Visit(NotExpression notExpression)
        {
            return !(bool)notExpression.Expression.Accept(this);
        }

        public object Visit(AdditionNode addNode)
        {
            return (dynamic)addNode.Lhs.Accept(this) + addNode.Rhs.Accept(this);
        }

        public object Visit(NegateExpression negateExpression)
        {
            return -(double)negateExpression.Expression.Accept(this);
        }

        public object Visit(SubtractNode subtractNode)
        {
            return (dynamic)subtractNode.Lhs.Accept(this) - subtractNode.Rhs.Accept(this);
        }

        public object Visit(MultiplyNode multiplyNode)
        {
            return (dynamic)multiplyNode.Lhs.Accept(this) * multiplyNode.Rhs.Accept(this);
        }

        public object Visit(DivideNode divideNode)
        {
            return (dynamic)divideNode.Lhs.Accept(this) / divideNode.Rhs.Accept(this);
        }
    }
}