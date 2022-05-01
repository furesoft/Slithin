using Newtonsoft.Json;

namespace SlithinMarketplace.Models;

public sealed class Grant
{
    [JsonProperty("grant_type")]
    public string GrantType { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }
}
