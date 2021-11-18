using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;

namespace Slithin.ActionCompiler.Compiling.Passes;

public class ConstantFoldingPass : IPass
{
    public CodeObject Process(CodeObject obj)
    {
        if (obj is BinaryOperator expr)
            return new Literal(Evaluate(expr));

        return obj;
    }

    private int Evaluate(CodeObject tree)
    {
        if (tree is BinaryOperator expr)
            switch (expr.Symbol)
            {
                case "+":
                    return Evaluate(expr.Left) + Evaluate(expr.Right);
                case "-":
                    return Evaluate(expr.Left) - Evaluate(expr.Right);
                case "*":
                    return Evaluate(expr.Left) * Evaluate(expr.Right);
                case "/":
                    return Evaluate(expr.Left) / Evaluate(expr.Right);
            }
        else if(tree is Literal lit)
        {
            return (int)lit.Value;
        }

        return 0;
    }
}