using ApiConsole.Core;
using CommandLine;
using Newtonsoft.Json;
using SlithinMarketplace.Models;

namespace ApiConsole.Commands;

[Verb("scripts", HelpText = "Act with scripts")]
public class ScriptsCommand : ICommand
{
    [Option('g', "getall", HelpText = "Flag for Getting all Templates")]
    public bool Get { get; set; }

    [Option('f', "file", HelpText = "The path of the file of the script")]
    public string ScriptFile { get; set; }

    [Option('s', "scriptinfo", HelpText = "The path of the file to describe the script as json")]
    public string ScriptInfoFile { get; set; }

    [Option('c', "create", HelpText = "Create Template and make upload request")]
    public bool UploadRequest { get; set; }

    public void Execute()
    {
        if (Get)
        {
            var templates = ServiceLocator.API.Get<Script[]>("scripts");

            Console.WriteLine(JsonConvert.SerializeObject(templates));
        }

        if (UploadRequest)
        {
            var script = JsonConvert.DeserializeObject<Script>(ScriptInfoFile);

            ServiceLocator.API.CreateAndUploadAsset(script, ScriptFile);
        }
    }
}
