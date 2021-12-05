using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Assignments;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Unary;

namespace Slithin.ActionCompiler.Compiling.Passes.Lowerer
{
    public class IncrementOperatorLowererPass : IPass
    {
        public CodeObject Process(CodeObject obj)
        {
            if (obj is PostIncrement incr)
            {
                return new Block(incr.Expression, new Assignment(incr.Expression, incr.Expression + 1));
            }
            else if (obj is PostDecrement decr)
            {
                return new Block(decr.Expression, new Assignment(decr.Expression, decr.Expression - 1));
            }
            else
            {
                return obj;
            }
        }
    }
}
