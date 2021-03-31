using Newtonsoft.Json;
using WDLT.Clients.Base;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JSearchItem
    {
        [JsonProperty("sell_listings")]
        public long Count { get; set; }

        [JsonProperty("sell_price")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))] 
        public double Price { get; set; }

        [JsonProperty("asset_description")]
        public JAsset Asset { get; set; }
    }
}