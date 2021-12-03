using Flo;
using System;
using System.Threading.Tasks;

namespace Slithin.ActionCompiler.Compiling.Stages
{
    public class ParsingStage : IHandler<CompilerContext, CompilerContext>
    {
        public async Task<CompilerContext> HandleAsync(CompilerContext input, Func<CompilerContext, Task<CompilerContext>> next)
        {
            foreach (var filename in input.Inputs)
            {
                var tree = OodParser.ParseFile(filename);

                input.Trees.Add(tree);
            }

            return await next.Invoke(input);
        }
    }
}
