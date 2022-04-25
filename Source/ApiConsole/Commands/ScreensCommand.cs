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

    [Option("data")]
    public string Json { get; set; }

    [Option('u', "upload", HelpText = "Create Screen and make upload request")]
    public bool UploadRequest { get; set; }

    public void Execute()
    {
        if (Get)
        {
            var screens = ServiceLocator.API.Get<Screen[]>("screens");

            Console.WriteLine(JsonConvert.SerializeObject(screens));
        }

        if (UploadRequest)
        {
            var request = ServiceLocator.API.CreateScreen(JsonConvert.DeserializeObject<Screen>(Json));
            Console.WriteLine(request);
        }
    }
}
