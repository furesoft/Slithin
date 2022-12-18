using LiteDB;

namespace Slithin.Modules.Repository.Models;

public class DatabaseAccessor
{
    public DatabaseAccessor(LiteDatabase dB)
    {
        DB = dB;
    }

    ~DatabaseAccessor()
    {
        DB.Dispose();
    }

    public LiteDatabase DB { get; set; }
}
