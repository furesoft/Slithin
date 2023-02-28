using System.Text.RegularExpressions;

namespace Slithin.Entities;

public partial struct IPAddress
{
    private IPAddress(string address, int port = 22)
    {
        Address = address;
        Port = port;
    }

    public string Address { get; set; }
    public int Port { get; set; }

    public static bool IsValid(string src)
    {
        return IpRegex().IsMatch(src);
    }

    public static IPAddress Parse(string src)
    {
        var match = IpRegex().Match(src);
        var result = new IPAddress();
        result.Port = 22;

        var splitted = src.Split(':', StringSplitOptions.RemoveEmptyEntries);

        result.Address = splitted[0];

        if (splitted.Length == 2)
        {
            result.Port = int.Parse(splitted[1]);
        }

        return result;
    }

    [GeneratedRegex("^(((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))(\\:([0-9]+))?$")]
    private static partial Regex IpRegex();
}
