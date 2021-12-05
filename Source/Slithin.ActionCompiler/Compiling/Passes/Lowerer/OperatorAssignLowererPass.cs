using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Arithmetic;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Assignments;
using System;

namespace Slithin.ActionCompiler.Compiling.Passes.Lowerer
{
    public class OperatorAssignLowererPass : IPass
    {
        public CodeObject Process(CodeObject obj)
        {
            return obj switch
            {
                AddAssign addass => new Assignment(addass.Left, addass.Left + addass.Right),
                SubtractAssign subass => new Assignment(subass.Left, subass.Left - subass.Right),
                _ => obj
            };
        }
    }
}
