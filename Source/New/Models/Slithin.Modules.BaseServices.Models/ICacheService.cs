namespace Slithin.Modules.BaseServices.Models;

public interface ICacheService
{
    void AddObject<T>(string name, T obj);

    T GetObject<T>(string name, Func<T>? objFactory = default);

    bool Contains(string name);
}
