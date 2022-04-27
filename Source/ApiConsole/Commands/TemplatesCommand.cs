using ApiConsole.Core;
using CommandLine;
using Newtonsoft.Json;
using Slithin.Marketplace.Models;

namespace ApiConsole.Commands;

[Verb("templates", HelpText = "Act with templates")]
public class TemplatesCommand : ICommand
{
    [Option('g', "getall", HelpText = "Flag for Getting all Templates")]
    public bool Get { get; set; }

    [Option('t', "template", HelpText = "The path of the file to describe the template as json")]
    public string TemplateInfoPath { get; set; }

    [Option('c', "create", HelpText = "Create Template and make upload request")]
    public bool UploadRequest { get; set; }

    public void Execute()
    {
        if (Get)
        {
            var templates = ServiceLocator.API.Get<Template[]>("templates");

            Console.WriteLine(JsonConvert.SerializeObject(templates));
        }

        if (UploadRequest)
        {
            var template = JsonConvert.DeserializeObject<Template>(TemplateInfoPath);
            template.SvgFileID = Guid.NewGuid().ToString();

            ServiceLocator.API.CreateAndUploadTemplate(template, TemplateInfoPath);
        }
    }
}
