using System.Globalization;
using Slithin.Scripting.Parsing;
using Slithin.Scripting.Parsing.AST;
using Slithin.Scripting.Parsing.AST.Expressions;
using Slithin.Scripting.Parsing.AST.Literals;

namespace Slithin.Scripting.Execution;

public partial class Interpreter : IVisitor<object>
{
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

    public object Visit(LiteralNode literal)
    {
        return literal.Value;
    }

    public object Visit(NowLiteralNode literal)
    {
        return DateTime.Now;
    }

    public object Visit(NameExpression nameExpression)
    {
        if (BindingTable.IsVariable(nameExpression.Name))
        {
            return BindingTable.GetVariable(nameExpression.Name); ;
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

    public object Visit(CallExpr callExpr)
    {
        var identifiers = ((NameExpression)callExpr.Identifiers);
        var callableName = identifiers.Name;

        if (BindingTable.IsCallable(callableName))
        {
            var arguments = new List<object>();
            foreach (var arg in callExpr.Arguments.Body)
            {
                arguments.Add(arg.Accept(this));
            }

            return BindingTable.Call(callableName, arguments.ToArray());
        }
        else
        {
            Messages.Add(Message.Error($"'{callableName}' is not callable.", identifiers.Line, identifiers.Column));

            return null;
        }
    }

    public object Visit(DayLiteralNode dayLiteral)
    {
        return DateTime.Today;
    }
}
