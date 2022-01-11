using Flo;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Slithin.ActionCompiler.Compiling.Stages
{
    public class ParsingStage : IHandler<CompilerContext, CompilerContext>
    {
        public async Task<CompilerContext> HandleAsync(CompilerContext context, Func<CompilerContext, Task<CompilerContext>> next)
        {
            bool hasError = false;
            foreach (var filename in context.InputFiles)
            {
                if (File.Exists(filename))
                {
                    var tree = OodParser.ParseFile(filename);

                    context.Trees.Add(tree);
                }
                else
                {
                    hasError = true;
                }
            }

            if (!hasError)
            {
                return await next.Invoke(context);
            }
            else
            {
                return context;
            }
        }
    }
}
