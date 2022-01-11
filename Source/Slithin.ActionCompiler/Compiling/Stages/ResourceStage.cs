using Flo;
using MessagePack;
using Newtonsoft.Json;
using Slithin.ModuleSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebAssembly;

namespace Slithin.ActionCompiler.Compiling.Stages
{
    public class ResourceStage : IHandler<CompilerContext, CompilerContext>
    {
        public async Task<CompilerContext> HandleAsync(CompilerContext context, Func<CompilerContext, Task<CompilerContext>> next)
        {
            var info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(context.MetadataFilename));
            var infoBytes = MessagePackSerializer.Serialize(info);
            context.ResultModule.CustomSections.Add(new CustomSection { Name = ".info", Content = new List<byte>(infoBytes) });

            if (context.ImageFilename != null)
                context.ResultModule.CustomSections.Add(new CustomSection
                { Name = ".image", Content = new List<byte>(File.ReadAllBytes(context.ImageFilename)) });

            if (context.UiFilename != null)
                context.ResultModule.CustomSections.Add(new CustomSection
                { Name = ".ui", Content = new List<byte>(File.ReadAllBytes(context.UiFilename)) });

            return await next.Invoke(context);
        }
    }
}
