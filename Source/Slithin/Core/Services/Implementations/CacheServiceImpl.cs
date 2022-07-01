using System;
using System.Collections.Concurrent;

namespace Slithin.Core.Services.Implementations;

public class CacheServiceImpl : ICacheService
{
    private readonly ConcurrentDictionary<string, object> _cache = new();

    public void AddObject<T>(string name, T obj)
    {
        _cache.AddOrUpdate(name, obj, (_, _) => obj);
    }

    public T GetObject<T>(string name, Func<T> obj = default)
    {
        if (_cache.ContainsKey(name))
        {
            return (T)_cache[name];
        }

        if (obj != null)
        {
            var instance = obj();
            AddObject(name, instance);

            return instance;
        }

        return default;
    }
}
