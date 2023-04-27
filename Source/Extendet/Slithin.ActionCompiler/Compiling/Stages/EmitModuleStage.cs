using Flo;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Slithin.ActionCompiler.Compiling.Stages
{
    public class EmitModuleStage : IHandler<CompilerContext, CompilerContext>
    {
        public async Task<CompilerContext> HandleAsync(CompilerContext context, Func<CompilerContext, Task<CompilerContext>> next)
        {
            using var stream = File.OpenWrite(context.OutputFilename);

            context.ResultModule.WriteToBinary(stream);

            return await next.Invoke(context);
        }
    }
}