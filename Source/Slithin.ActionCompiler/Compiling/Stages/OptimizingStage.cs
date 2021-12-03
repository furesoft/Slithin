using Flo;
using Slithin.ActionCompiler.Compiling.Passes;
using System;
using System.Threading.Tasks;

namespace Slithin.ActionCompiler.Compiling.Stages
{
    public class OptimizingStage : IHandler<CompilerContext, CompilerContext>
    {
        public OptimizingStage()
        {
            Optimizer.AddPass<ConstantFoldingPass>();
        }

        public async Task<CompilerContext> HandleAsync(CompilerContext input, Func<CompilerContext, Task<CompilerContext>> next)
        {
            for (int i = 0; i < input.Trees.Count; i++)
            {
                input.Trees[i] = Optimizer.Process(input.Trees[i]);
            }

            return await next.Invoke(input);
        }
    }
}
