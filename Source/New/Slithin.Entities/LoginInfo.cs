using LiteDB;

namespace Slithin.Entities;

public record LoginInfo
{
    public ObjectId _id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string IP { get; set; } = string.Empty;

    public byte[] Key { get; set; }

    [BsonIgnore]
    public bool UsesKey => Key != null;

    public override string ToString()
    {
        return Name;
    }
}
