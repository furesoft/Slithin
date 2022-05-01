using ApiConsole.Commands;
using ApiConsole.Core;
using CommandLine;

namespace ApiConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            if (args[0] == "--interactive" || args[0] == "-i")
            {
                while (true)
                {
                    Console.Write("> ");
                    var cmd = Console.ReadLine();

                    InvokeCommand(cmd.Split(' '));
                }
            }
        }

        InvokeCommand(args);
    }

    private static void InvokeCommand(string[] args)
    {
        Parser.Default
                    .ParseArguments<LoginCommand, ScreensCommand, TemplatesCommand>(args)
                    .WithParsed<ICommand>(t => t.Execute());
    }
}
