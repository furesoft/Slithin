using ApiConsole.Core;
using CommandLine;
using Newtonsoft.Json;
using SlithinMarketplace.Models;

namespace ApiConsole.Commands;

[Verb("screens", HelpText = "Act with screens")]
public class ScreensCommand : ICommand
{
    [Option('f', "filename", HelpText = "The filename of the screen. Example: suspend.jpg")]
    public string Filename { get; set; }

    [Option('u', "upload", HelpText = "The path of the file to use as image")]
    public string FileToUpload { get; set; }

    [Option('g', "getall", HelpText = "Flag for Getting all Screens")]
    public bool Get { get; set; }

    [Option('n', "name")]
    public string Name { get; set; }

    [Option('c', "create", HelpText = "Create Screen and make upload request")]
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
            var screen = new Screen
            {
                name = Name,
                filename = Filename
            };

            ServiceLocator.API.CreateAndUploadAsset(screen, FileToUpload);
        }
    }
}
