using ApiConsole.Core;
using CommandLine;

namespace ApiConsole.Commands;

[Verb("files", HelpText = "Act with files")]
public class FilesCommand : ICommand
{
    [Option('d', "download", HelpText = "Flag for downloading a file")]
    public bool Download { get; set; }

    [Option('f', "fiilename", HelpText = "filename and path to save the file to")]
    public string Filename { get; set; }

    [Option('i', "id", HelpText = "File ID to download")]
    public string ID { get; set; }

    public void Execute()
    {
        if (Download)
        {
            ServiceLocator.API.DownloadFile(ID, Filename);
        }
    }
}
