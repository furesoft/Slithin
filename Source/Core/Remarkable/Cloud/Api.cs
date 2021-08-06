using System.IO;
using RestSharp;
using JsonSerializer = RestSharp.Serialization.Json.JsonSerializer;

namespace Slithin.Core.Remarkable.Cloud
{
    public class Api
    {
        public static string DeviceID = "9AB7CCA8-1B6D-4AAC-A07C-9831DF472D69";

        public static string Authenticate(string otc)
        {
            var client = new RestClient("https://webapp-production-dot-remarkable-production.appspot.com/");

            client.UseSerializer(
                () => new JsonSerializer { DateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ" }
            );
            var request = new RestRequest("token/json/2/device/new", Method.POST, DataFormat.Json);
            request.AddHeader("Authorization", "Bearer ");

            var body = new
            {
                code = otc,
                deviceDesc = "desktop-windows",
                deviceID = DeviceID
            };

            request.AddJsonBody(body);

            var response = client.Post(request);

            var file = Path.Combine(ServiceLocator.ConfigBaseDir, ".token");

            File.WriteAllText(file, response.Content);

            return response.Content;
        }

        public static string GetStorageUrl()
        {
            var client = new RestClient("https://service-manager-production-dot-remarkable-production.appspot.com");

            client.UseSerializer(
                () => new JsonSerializer { DateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ" }
            );
            var request = new RestRequest("/service/json/1/document-storage", Method.GET, DataFormat.Json);
            request.AddQueryParameter("environment", "production");
            request.AddQueryParameter("group", "auth0|5a68dc51cb30df3877a1d7c4");
            request.AddQueryParameter("apiVer", "2");

            var response = client.Get<GetStorageResult>(request);

            return "https://" + response.Data.Host;
        }

        public static string GetToken(string otc = null)
        {
            var file = Path.Combine(ServiceLocator.ConfigBaseDir, ".token");
            if (!File.Exists(file))
            {
                File.WriteAllText(file, Authenticate(otc));
            }

            return File.ReadAllText(file);
        }

        public static string RefreshToken()
        {
            var client = new RestClient("https://webapp-production-dot-remarkable-production.appspot.com/");

            client.UseSerializer(
                () => new JsonSerializer { DateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ" }
            );
            var request = new RestRequest("/token/json/2/user/new", Method.POST, DataFormat.Json);
            request.AddHeader("Authorization", "Bearer " + File.ReadAllText(Path.Combine(ServiceLocator.ConfigBaseDir, ".token")));

            var response = client.Post(request);

            var content = response.Content;

            return content;
        }
    }
}
