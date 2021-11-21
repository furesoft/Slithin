namespace Slithin.Core.Services;

public interface ICacheService
{
    void AddObject<T>(string name, T obj);

    public T GetObject<T>(string name, T obj = default);
}
