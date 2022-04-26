using ApiConsole.Commands;
using ApiConsole.Core;
using CommandLine;

namespace ApiConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            var cmd = Console.ReadLine();

            Parser.Default
            .ParseArguments<LoginCommand, ScreensCommand, TemplatesCommand>(cmd.Split(' '))
            .WithParsed<ICommand>(t => t.Execute());
        }
    }
}
