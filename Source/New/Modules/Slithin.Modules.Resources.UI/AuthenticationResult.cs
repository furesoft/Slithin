using Newtonsoft.Json;

namespace Slithin.Modules.Resources.UI;

public class AuthenticationResult
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
}
