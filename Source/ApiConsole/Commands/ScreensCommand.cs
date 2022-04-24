using ApiConsole.Core;
using CommandLine;

namespace ApiConsole.Commands;

[Verb("screens", HelpText = "Act with screens")]
public class ScreensCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Executing screens");
    }
}
