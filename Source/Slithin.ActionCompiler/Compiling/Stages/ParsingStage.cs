using Flo;
using System;
using System.Threading.Tasks;

namespace Slithin.ActionCompiler.Compiling.Stages
{
    public class ParsingStage : IHandler<CompilerContext, CompilerContext>
    {
        public async Task<CompilerContext> HandleAsync(CompilerContext context, Func<CompilerContext, Task<CompilerContext>> next)
        {
            foreach (var filename in context.InputFiles)
            {
                var tree = OodParser.ParseFile(filename);

                context.Trees.Add(tree);
            }

            return await next.Invoke(context);
        }
    }
}
