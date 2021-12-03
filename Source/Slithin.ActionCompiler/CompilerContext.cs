using Furesoft.Core.CodeDom.CodeDOM;
using System.Collections.Generic;
using WebAssembly;

namespace Slithin.ActionCompiler;

public class CompilerContext
{
    public List<CodeUnit> Trees { get; set; } = new();

    public List<string> Inputs { get; set; } = new();

    public string OutputFilename { get; set; }

    public Module ResultModule { get; set; } = new();
}
