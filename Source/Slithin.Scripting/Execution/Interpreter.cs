using Slithin.Scripting.Parsing;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Statements;

namespace Slithin.Scripting.Execution;

public partial class Interpreter : IVisitor<object>
{
    public BindingTable BindingTable { get; set; } = new();
    public List<Message> Messages { get; set; } = new();

    public object Visit(InvalidNode invalidNode)
    {
        throw new NotImplementedException();
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

    public object Visit(ExpressionStatement expressionStatement)
    {
        return expressionStatement.Expression.Accept(this);
    }

    public object Visit(RememberStatement rememberStatement)
    {
        var value = rememberStatement.Value.Accept(this);

        if (!BindingTable.IsCallable(rememberStatement.Name))
        {
            BindingTable.AddVariable(rememberStatement.Name, value);
        }

        return null;
    }

    public object Visit(AssignmentStatement assignmentStatement)
    {
        if (BindingTable.IsVariable(assignmentStatement.NameToken.Text))
        {
            BindingTable.AddVariable(assignmentStatement.NameToken.Text, assignmentStatement.Value.Accept(this));
        }

        return null;
    }

    public object Visit(Expression expression)
    {
        return expression.Accept(this);
    }
}
