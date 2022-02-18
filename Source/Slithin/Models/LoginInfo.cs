using LiteDB;

namespace Slithin.Models;

public record LoginInfo
{
    public ObjectId _id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string IP { get; set; } = string.Empty;

    public override string ToString()
    {
        return Name;
    }
}
