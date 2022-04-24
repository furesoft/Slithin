using ApiConsole.Core;
using CommandLine;

namespace ApiConsole.Commands;

[Verb("templates", HelpText = "Act with templates")]
public class TemplatesCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Executing templates");
    }
}
