using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;

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
    }
}