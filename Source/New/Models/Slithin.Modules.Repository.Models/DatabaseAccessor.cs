using LiteDB;

namespace Slithin.Modules.Repository.Models;

public class DatabaseAccessor
{
    public DatabaseAccessor(LiteDatabase dB)
    {
        DB = dB;
        DB.BeginTrans();
    }

    ~DatabaseAccessor()
    {
        DB.Commit();
    }

    public LiteDatabase DB { get; set; }
}
