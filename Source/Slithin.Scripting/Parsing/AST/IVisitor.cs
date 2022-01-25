using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Binary;
using Slithin.Scripting.Parsing.AST.Expressions.Unary;

namespace Slithin.Scripting.Parsing.AST;

public interface IVisitor<T>
{
    T Visit(InvalidNode invalidNode);

    T Visit(LiteralNode literal);

    T Visit(CompilationUnit compilationUnit);
    T Visit(GroupExpression groupExpression);
    T Visit(Block block);

    T Visit(NotExpression notExpression);

    T Visit(InvalidExpr invalidExpr);

    T Visit(AdditionNode addNode);

    T Visit(NegateExpression negateExpression);

    T Visit(SubtractNode subtractNode);

    T Visit(NameExpression nameExpression);

    T Visit(MultiplyNode multiplyNode);

    T Visit(DivideNode divideNode);
}