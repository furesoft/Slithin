using System.Collections.Concurrent;
using Slithin.Modules.Cache.Models;

namespace Slithin.Modules.Caching;

internal class CacheServiceImpl : ICacheService
{
    private readonly ConcurrentDictionary<string, object> _cache = new();

    public void AddObject<T>(string name, T obj)
    {
        _cache.AddOrUpdate(name, obj!, (_, _) => obj!);
    }

    public bool Contains(string name)
    {
        return _cache.ContainsKey(name);
    }

    public T GetObject<T>(string name, Func<T>? objFactory = default!)
    {
        if (_cache.TryGetValue(name, out var value))
        {
            return (T)value;
        }

        if (objFactory == null)
        {
            return default!;
        }

        var instance = objFactory();
        AddObject(name, instance);

        return instance;

    }
}
