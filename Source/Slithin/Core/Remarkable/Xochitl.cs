using Renci.SshNet;

namespace Slithin.Core.Remarkable;

public class Xochitl
{
    private readonly SshClient _client;

    public Xochitl(SshClient client)
    {
        _client = client;
    }

    public bool GetIsBeta()
    {
        var str = GetProperty("BetaProgram");

        if (!string.IsNullOrEmpty(str))
        {
            return bool.Parse(str);
        }

        return false;
    }

    private string GetProperty(string key)
    {
        var sshCommand = _client.RunCommand($"grep '^{key}' /home/root/.config/remarkable/xochitl.conf");
        var str = sshCommand.Result;

        return str.Replace($"{key}=", "").Replace("\n", "");
    }
}
