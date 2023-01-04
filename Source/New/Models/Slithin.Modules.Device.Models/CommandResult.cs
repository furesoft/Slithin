namespace Slithin.Modules.Device.Models;

public class CommandResult
{
    public CommandResult(string error, string result)
    {
        Error = error;
        Result = result;
    }

    public string Error { get; set; }
    public string Result { get; set; }
}
