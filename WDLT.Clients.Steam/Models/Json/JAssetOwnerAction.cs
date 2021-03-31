using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JAssetOwnerAction
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}