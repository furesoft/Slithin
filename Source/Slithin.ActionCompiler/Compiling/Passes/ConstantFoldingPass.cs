using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Statements.Variables.Base;

namespace Slithin.ActionCompiler.Compiling.Passes;

public class ConstantFoldingPass : IPass
{
    public CodeObject Process(CodeObject obj, PassManager passManager)
    {
        if (obj is VariableDecl ass && ass.Initialization is BinaryOperator expr)
        {
            int? result = Evaluate(expr);
            if (result.HasValue)
            {
                ass.Initialization = new Literal(result);
            }
        }

        return obj;
    }

    private int? Evaluate(CodeObject tree)
    {
        if (tree is BinaryOperator expr)
        {
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
        }
        else if (tree is Literal lit)
        {
            return int.Parse(lit.Text);
        }

        return null;
    }
}