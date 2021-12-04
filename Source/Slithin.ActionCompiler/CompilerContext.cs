using CommandLine;
using Furesoft.Core.CodeDom.CodeDOM;
using System.Collections.Generic;
using WebAssembly;

namespace Slithin.ActionCompiler;

public class CompilerContext
{
    public List<CodeUnit> Trees { get; set; } = new();

    [Option('i', "input", Required = true, HelpText = "Input files to be compiled.")]
    public IEnumerable<string> InputFiles { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output filename")]
    public string OutputFilename { get; set; }

    [Option('u', "ui", Required = true, HelpText = "UI filename")]
    public string UiFilename { get; set; }

    [Option('m', "metadata", Required = true, HelpText = "Metadata filename")]
    public string MetadataFilename { get; set; }

    [Option('s', "symbol", Required = true, HelpText = "Symbolicon filename")]
    public string ImageFilename { get; set; }

    public Module ResultModule { get; set; } = new();
}
