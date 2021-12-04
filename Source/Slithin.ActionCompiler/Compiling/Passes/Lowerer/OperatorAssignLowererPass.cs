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
            if (obj is AddAssign addass)
            {
                return new Assignment(addass.Left, new Add(addass.Left, addass.Right));
            }
            if (obj is SubtractAssign subass)
            {
                return new Assignment(subass.Left, new Subtract(subass.Left, subass.Right));
            }
            else
            {
                return obj;
            }
        }
    }
}
