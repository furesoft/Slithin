using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Binary;
using Slithin.Scripting.Parsing.AST.Expressions.Unary;

namespace Slithin.Scripting.Execution
{
    internal class Interpreter : IVisitor<object>
    {
        public object Visit(InvalidNode invalidNode)
        {
            throw new NotImplementedException();
        }

        public object Visit(LiteralNode literal)
        {
            throw new NotImplementedException();
        }

        public object Visit(CompilationUnit compilationUnit)
        {
            throw new NotImplementedException();
        }

        public object Visit(Block block)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public object Visit(AdditionNode addNode)
        {
            throw new NotImplementedException();
        }

        public object Visit(NegateExpression negateExpression)
        {
            throw new NotImplementedException();
        }

        public object Visit(SubtractNode subtractNode)
        {
            throw new NotImplementedException();
        }

        public object Visit(MultiplyNode multiplyNode)
        {
            throw new NotImplementedException();
        }

        public object Visit(DivideNode divideNode)
        {
            throw new NotImplementedException();
        }
    }
}