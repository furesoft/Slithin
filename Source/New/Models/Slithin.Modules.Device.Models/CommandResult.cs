namespace Slithin.Modules.Device.Models;

public class CommandResult
{
    public CommandResult(string error, string result)
    {
        Error = error;
        Result = result;
    }

    public CommandResult(string result)
    {
        Result = result;
        Error = string.Empty;
    }

    public string Error { get; }
    public string Result { get; }
}
