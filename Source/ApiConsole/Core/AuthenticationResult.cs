using Newtonsoft.Json;

namespace ApiConsole.Core;

public class AuthenticationResult
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}
