using System.Collections.Specialized;

namespace SlithinMarketplace;

public static class Extensions
{
    public static IEnumerable<T> FilterByQuery<T>(this IEnumerable<T> collection, NameValueCollection parameters)
    {
        if (parameters["skip"] != null)
        {
            collection = collection.Skip(int.Parse(parameters["skip"]));
        }

        if (parameters["count"] != null)
        {
            collection = collection.Take(int.Parse(parameters["count"]));
        }

        return collection;
    }
}
