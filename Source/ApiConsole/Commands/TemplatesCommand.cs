using ApiConsole.Core;
using CommandLine;

namespace ApiConsole.Commands;

[Verb("templates", HelpText = "Act with templates")]
public class TemplatesCommand : ICommand
{
    [Option('u', "upload", HelpText = "The path of the file to use as image")]
    public string FileToUpload { get; set; }

    [Option('g', "getall", HelpText = "Flag for Getting all Templates")]
    public bool Get { get; set; }

    [Option('c', "create", HelpText = "Create Template and make upload request")]
    public bool UploadRequest { get; set; }

    public void Execute()
    {
        Console.WriteLine("Executing templates");
    }
}
