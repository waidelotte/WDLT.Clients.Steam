using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JPriceOverview : JSuccess<bool>
    {
        [JsonProperty("lowest_price")]
        public string LowestPrice { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("median_price")]
        public string MedianPrice { get; set; }
    }
}