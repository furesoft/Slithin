using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Literals;
using Slithin.Scripting.Parsing.AST.Statements;

namespace Slithin.Scripting.Parsing.AST;

public interface IVisitor<T>
{
    T Visit(InvalidNode invalidNode);

    T Visit(LiteralNode literal);

    T Visit(NowLiteralNode literal);

    T Visit(ExpressionStatement expressionStatement);

    T Visit(CompilationUnit compilationUnit);

    T Visit(AssignmentStatement assignmentStatement);

    T Visit(BinaryExpression binaryExpression);

    T Visit(UnaryExpression unaryExpression);

    T Visit(GroupExpression groupExpression);

    T Visit(Block block);

    T Visit(InvalidExpr invalidExpr);

    T Visit(NameExpression nameExpression);

    T Visit(RememberStatement rememberStatement);

    T Visit(CallExpr callExpr);

    T Visit(DayLiteralNode dayLiteral);
    T Visit(Expression expression);
}
