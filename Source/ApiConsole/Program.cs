using ApiConsole.Commands;
using ApiConsole.Core;
using CommandLine;

namespace ApiConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default
            .ParseArguments<LoginCommand, ScreensCommand, TemplatesCommand>(args).WithParsed<ICommand>(t => t.Execute());
    }
}
