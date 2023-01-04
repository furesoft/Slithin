using LiteDB;

namespace Slithin.Modules.Repository.Models;

public class DatabaseAccessor
{
    private LiteDatabase _db;

    public DatabaseAccessor(LiteDatabase dB)
    {
        _db = dB;
    }

    ~DatabaseAccessor()
    {
        _db.Checkpoint();
    }

    public void Delete<T>(ObjectId id)
    {
        _db.GetCollection<T>().Delete(id);
    }

    public ILiteQueryable<T> Query<T>()
    {
        return _db.GetCollection<T>().Query();
    }

    public IEnumerable<T> FindAll<T>()
    {
        return _db.GetCollection<T>().FindAll();
    }

    public void Insert<T>(T obj)
    {
        _db.GetCollection<T>().Insert(obj);
    }

    public void Update<T>(T obj)
    {
        _db.GetCollection<T>().Update(obj);
    }

    public void Upsert<T>(T obj)
    {
        _db.GetCollection<T>().Upsert(obj);
    }

    public ILiteCollection<T> GetCollection<T>()
    {
        return _db.GetCollection<T>();
    }
}
