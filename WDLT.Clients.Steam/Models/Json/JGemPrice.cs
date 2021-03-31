using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JGemPrice : JSuccess<bool>
    {
        [JsonProperty("goo_value")]
        public int Price { get; set; }
    }
}