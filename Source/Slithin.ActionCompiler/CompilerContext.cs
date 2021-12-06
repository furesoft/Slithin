using CommandLine;
using Furesoft.Core.CodeDom.CodeDOM;
using System.Collections.Generic;
using WebAssembly;

namespace Slithin.ActionCompiler;

public class CompilerContext
{
    [Option('s', "symbol", HelpText = "Symbolicon filename")]
    public string ImageFilename { get; set; }

    [Option('i', "input", Required = true, HelpText = "Input files to be compiled.")]
    public IEnumerable<string> InputFiles { get; set; }

    [Option('l', "library", HelpText = "The compiled module is a library")]
    public bool IsLibray { get; set; }

    [Option('m', "metadata", Required = true, HelpText = "Metadata filename")]
    public string MetadataFilename { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output filename")]
    public string OutputFilename { get; set; }

    [Option('r', "reference", Required = false, HelpText = "Reference that should be linked")]
    public IEnumerable<string> References { get; set; }

    public Module ResultModule { get; set; } = new();
    public List<CodeUnit> Trees { get; set; } = new();

    [Option('u', "ui", Required = true, HelpText = "UI filename")]
    public string UiFilename { get; set; }
}