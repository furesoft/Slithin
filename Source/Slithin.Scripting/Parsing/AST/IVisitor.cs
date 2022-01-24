using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST;

namespace Slithin.Scripting.Parsing.AST;

public interface IVisitor<T>
{
    T Visit(InvalidNode invalidNode);

    T Visit(LiteralNode literal);

    T Visit(CompilationUnit compilationUnit);

    T Visit(Block block);

    T Visit(InvalidExpr invalidExpr);

    T Visit(NameExpression nameExpression);
}
