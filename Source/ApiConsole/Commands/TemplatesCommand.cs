using ApiConsole.Core;
using CommandLine;
using Newtonsoft.Json;
using Slithin.Marketplace.Models;

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
        if (Get)
        {
            var screens = ServiceLocator.API.Get<Template[]>("screens");

            Console.WriteLine(JsonConvert.SerializeObject(screens));
        }

        if (UploadRequest)
        {
            /*var screen = new Template
            {
                name = Name,
                filename = Filename
            };

            ServiceLocator.API.CreateAndUploadAsset(screen, FileToUpload);*/
        }
    }
}
