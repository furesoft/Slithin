using ApiConsole.Core;
using CommandLine;
using Newtonsoft.Json;
using SlithinMarketplace.Models;

namespace ApiConsole.Commands;

[Verb("screens", HelpText = "Act with screens")]
public class ScreensCommand : ICommand
{
    [Option('g', "getall", HelpText = "Flag for Getting all Screens")]
    public bool Get { get; set; }

    public void Execute()
    {
        if (Get)
        {
            var screens = ServiceLocator.API.Stream<Screen>().Get();

            Console.WriteLine(JsonConvert.SerializeObject(screens));
        }
    }
}
