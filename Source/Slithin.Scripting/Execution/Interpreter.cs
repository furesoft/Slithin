using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Expressions.Binary;
using Slithin.Scripting.Parsing.AST.Expressions.Unary;
using Slithin.Scripting.Parsing.AST.Statements;

namespace Slithin.Scripting.Execution;

public class Interpreter : IVisitor<object>
{
    public Dictionary<string, object> Variables { get; set; } = new();

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
        object value = null;
        foreach (var node in block.Body)
        {
            value = node.Accept(this);
        }

        return value;
    }

    public object Visit(InvalidExpr invalidExpr)
    {
        throw new NotImplementedException();
    }

    public object Visit(NameExpression nameExpression)
    {
        if (Variables.ContainsKey(nameExpression.Name))
        {
            return Variables[nameExpression.Name];
        }

        return null;
    }

    public object Visit(GroupExpression groupExpression)
    {
        return groupExpression.Inner.Accept(this);
    }

    public object Visit(NotExpression notExpression)
    {
        return !(bool)notExpression.Expression.Accept(this);
    }

    public object Visit(AdditionNode addNode)
    {
        return (dynamic)addNode.Lhs.Accept(this) + (dynamic)addNode.Rhs.Accept(this);
    }

    public object Visit(NegateExpression negateExpression)
    {
        return -(double)negateExpression.Expression.Accept(this);
    }

    public object Visit(SubtractNode subtractNode)
    {
        return (dynamic)subtractNode.Lhs.Accept(this) - (dynamic)subtractNode.Rhs.Accept(this);
    }

    public object Visit(MultiplyNode multiplyNode)
    {
        return (dynamic)multiplyNode.Lhs.Accept(this) * (dynamic)multiplyNode.Rhs.Accept(this);
    }

    public object Visit(DivideNode divideNode)
    {
        return (dynamic)divideNode.Lhs.Accept(this) / (dynamic)divideNode.Rhs.Accept(this);
    }

    public object Visit(ExpressionStatement expressionStatement)
    {
        return expressionStatement.Expression.Accept(this);
    }

    public object Visit(RememberStatement rememberStatement)
    {
        var value = rememberStatement.Value.Accept(this);

        if (!Variables.ContainsKey(rememberStatement.NameToken.Text))
        {
            Variables.Add(rememberStatement.NameToken.Text, value);
        }

        return null;
    }
}
