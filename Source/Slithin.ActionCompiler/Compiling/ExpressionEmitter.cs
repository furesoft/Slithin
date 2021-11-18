using System.Collections.Generic;
using System.Linq;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Unary.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Other;
using WebAssembly;
using WebAssembly.Instructions;

namespace Slithin.ActionCompiler.Compiling;

public static class ExpressionEmitter
{
    public static List<Instruction> Emit(Expression expr, Scope scope)
    {
        var result = new List<Instruction>();

        if (expr is BinaryOperator binary)
        {
            result.AddRange(Emit(binary.Left, scope));
            result.AddRange(Emit(binary.Right, scope));

            switch (binary.Symbol)
            {
                case "+":
                {
                    if (binary.Left is Literal lit && binary.Right is Literal lit2)
                    {
                        switch (lit.Value)
                        {
                            case int when lit2.Value is int:
                                result.Add(new Int32Add());
                                break;
                            case long when lit2.Value is long:
                                result.Add(new Int64Add());
                                break;
                            case float when lit2.Value is float:
                                result.Add(new Float32Add());
                                break;
                            case double when lit2.Value is double:
                                result.Add(new Float64Add());
                                break;
                        }
                    }

                    break;
                }
            }
        }
        else if (expr is UnaryOperator unary)
        { 
            result.Add(Emit(unary.Expression, scope).First());
            
            switch (unary.Symbol)
            {
                case "-":
                {
                    

                    break;
                }
            }
        }
        else if (expr is Literal literal)
        {
            if (literal.Value is int intValue)
            {
                result.Add(new Int32Constant(intValue));
            }
            else if (literal.Value is float doubleValue)
            {
                result.Add(new Float32Constant(doubleValue));
            }
        }
        else if (expr is UnresolvedRef {Reference: string name})
        {
            if (scope.IsParameter(name))
            {
                result.Add(new LocalGet(scope.GetParameterIndex(name)));
            }
        }

        return result;
    }
}