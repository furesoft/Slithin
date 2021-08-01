using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace Slithin.Core.Remarkable.Cloud
{
    public static class Storage
    {
        public static string Download(string blobUrl)
        {
            var tmp = Path.GetTempFileName();

            var wc = new WebClient();

            wc.DownloadFile(blobUrl, tmp);

            return tmp;
        }

        public static void Extract(string path, string destination)
        {
            ZipFile.ExtractToDirectory(path, destination);
        }

        public static ListItemResult[] ListItems()
        {
            var storageUri = Api.GetStorageUrl();

            var client = new RestClient(storageUri);
            var request = new RestRequest("/document-storage/json/2/docs", Method.GET, DataFormat.Json);
            request.AddQueryParameter("withBlob", "true");
            var token = Api.RefreshToken();

            request.AddHeader("Authorization", "Bearer " + token);

            var response = client.Get(request);
            var obj = JsonConvert.DeserializeObject<ListItemResult[]>(response.Content);

            return obj;
        }
    }
}
