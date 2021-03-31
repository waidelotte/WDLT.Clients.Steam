using Newtonsoft.Json;
using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JBooster   
    {
        [JsonProperty("appid")]
        public EApp App { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("series")]
        public int Series { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("unavailable")]
        public bool Unavailable { get; set; }
    }
}