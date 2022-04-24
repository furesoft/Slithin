using ApiConsole.Core;
using CommandLine;

namespace ApiConsole.Commands;

[Verb("login", HelpText = "Login to the marketplace api")]
public class LoginCommand : ICommand
{
    [Option('p', "password", Required = true, HelpText = "The password to login with")]
    public string Password { get; set; }

    [Option('u', "username", Required = true, HelpText = "The username to login with")]
    public string Username { get; set; }

    public void Execute()
    {
        Console.WriteLine("Executing login");
    }
}
