using RestSharp;

namespace ApiConsole;

public sealed class AssetStream<T>
{
    private RestClient _client;
    private RestRequest _request;

    public AssetStream(RestClient client, RestRequest request)
    {
        _client = client;
        _request = request;
    }

    public AssetStream<T> First()
    {
        return Take(1);
    }

    public IEnumerable<T> Get()
    {
        return GetAll();
    }

    public AssetStream<T> Skip(int count)
    {
        _request.AddQueryParameter("count", count);

        return this;
    }

    public AssetStream<T> Take(int count)
    {
        _request.AddQueryParameter("count", count);

        return this;
    }

    private IEnumerable<T> GetAll()
    {
        return _client.GetAsync<List<T>>(_request).Result;
    }
}
