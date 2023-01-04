using System.Text.RegularExpressions;

namespace Slithin.Entities;

public struct IPAddress
{
    public const string Pattern = @"^(((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))(\:([0-9]+))?$";

    public IPAddress()
    {
        Port = 22;
        Address = "0.0.0.0";
    }

    public string Address { get; set; }
    public int Port { get; set; }

    public static bool IsValid(string src)
    {
        return Regex.IsMatch(src, Pattern);
    }

    public static IPAddress Parse(string src)
    {
        var match = Regex.Match(src, Pattern);
        var result = new IPAddress();

        if (match.Success)
        {
            var splitted = src.Split(':', StringSplitOptions.RemoveEmptyEntries);

            result.Address = splitted[0];

            if (splitted.Length == 2)
            {
                result.Port = int.Parse(splitted[1]);
            }
        }
        else
        {
            throw new FormatException("IP Address is not in the correct format");
        }

        return result;
    }
}
