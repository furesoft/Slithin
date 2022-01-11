using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Assignments;

namespace Slithin.ActionCompiler.Compiling.Passes.Lowerer
{
    public class OperatorAssignLowererPass : IPass
    {
        public CodeObject Process(CodeObject obj, PassManager passManager)
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
