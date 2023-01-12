using System.ComponentModel;
using LiteDB;

namespace Slithin.Entities;

public record LoginInfo : INotifyPropertyChanged
{
    public ObjectId? _id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    private string _ip = string.Empty;

    public string IP
    {
        get { return _ip; }
        set
        {
            _ip = value;
            PropertyChanged?.Invoke(this, new(nameof(IP)));
        }
    }

    public byte[] Key { get; set; }

    [BsonIgnore]
    public bool UsesKey => Key != null;

    public event PropertyChangedEventHandler? PropertyChanged;

    public override string ToString()
    {
        return Name;
    }
}
