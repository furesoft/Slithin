using Newtonsoft.Json;

namespace Slithin.API.Lib;

public class AuthenticationResult
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}
