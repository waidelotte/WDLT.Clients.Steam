using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JListing
    {
        [JsonProperty("listingid")]
        public long Id { get; set; }

        [JsonProperty("asset")]
        public JAsset Asset { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("publisher_fee_percent")]
        public decimal PublisherFeePercent { get; set; }

        [JsonProperty("converted_price_per_unit")]
        public long PricePerUnit { get; set; }

        [JsonProperty("converted_fee_per_unit")]
        public long FeePerUnit { get; set; }
    }
}