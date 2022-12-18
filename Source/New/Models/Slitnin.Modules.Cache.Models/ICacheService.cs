namespace Slithin.Modules.Cache.Models;

public interface ICacheService
{
    void AddObject<T>(string name, T obj);

    public T GetObject<T>(string name, Func<T> obj = default);
}
