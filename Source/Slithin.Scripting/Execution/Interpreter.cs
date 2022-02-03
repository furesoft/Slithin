using System.Globalization;
using Slithin.Scripting.Parsing;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Statements;

namespace Slithin.Scripting.Execution;

public class Interpreter : IVisitor<object>
{
    public List<Message> Messages { get; set; } = new();
    public Dictionary<string, object> Variables { get; set; } = new();

    public object EvaluateDate(UnaryExpression dateExpression)
    {
        return DateTime.Parse(dateExpression.Expression.ToString(), CultureInfo.InvariantCulture);
    }

    public object EvaluateNegation(UnaryExpression negateExpression)
    {
        return -(double)negateExpression.Expression.Accept(this);
    }

    public object EvaluateNot(UnaryExpression notExpression)
    {
        return !(bool)notExpression.Expression.Accept(this);
    }

    public object EvaluateTime(BinaryExpression timeNode)
    {
        return TimeSpan.Parse(timeNode.ToString());
    }

    public object Visit(InvalidNode invalidNode)
    {
        throw new NotImplementedException();
    }

    public object Visit(LiteralNode literal)
    {
        return literal.Value;
    }

    public object Visit(NowLiteralNode literal)
    {
        return DateTime.Now;
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
        else
        {
            Messages.Add(Message.Error($"Variable '{nameExpression.Name}' not found.", nameExpression.Line, nameExpression.Column));
        }

        return null;
    }

    public object Visit(GroupExpression groupExpression)
    {
        return groupExpression.Inner.Accept(this);
    }

    public object Visit(ExpressionStatement expressionStatement)
    {
        return expressionStatement.Expression.Accept(this);
    }

    public object Visit(RememberStatement rememberStatement)
    {
        var value = rememberStatement.Value.Accept(this);

        if (!Variables.ContainsKey(rememberStatement.Name))
        {
            Variables.Add(rememberStatement.Name, value);
        }

        return null;
    }

    public object Visit(BinaryExpression binaryExpression)
    {
        var left = (dynamic)binaryExpression.Lhs.Accept(this);
        var right = (dynamic)binaryExpression.Rhs.Accept(this);

        return binaryExpression.OperatorToken.Type switch
        {
            TokenType.Plus => left + right,
            TokenType.Minus => left - right,
            TokenType.Star => left * right,
            TokenType.Slash => left / right,
            TokenType.Colon => EvaluateTime(binaryExpression),
            _ => throw new NotImplementedException()
        };
    }

    public object Visit(UnaryExpression unaryExpression)
    {
        return unaryExpression.OperatorToken.Type switch
        {
            TokenType.At => EvaluateDate(unaryExpression),
            TokenType.Minus => EvaluateNegation(unaryExpression),
            TokenType.Not => EvaluateNot(unaryExpression),
            _ => throw new NotImplementedException()
        };
    }

    public object Visit(AssignmentStatement assignmentStatement)
    {
        if (Variables.ContainsKey(assignmentStatement.NameToken.Text))
        {
            Variables[assignmentStatement.NameToken.Text] = assignmentStatement.Value.Accept(this);
        }

        return null;
    }
}
