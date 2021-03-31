using Newtonsoft.Json;
using WDLT.Clients.Base;
using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JWallet
    {
        [JsonProperty("wallet_currency")]
        public ECurrency Currency { get; set; }

        [JsonProperty("wallet_country")]
        public string Country { get; set; }

        [JsonProperty("wallet_fee")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double Fee { get; set; }

        [JsonProperty("wallet_fee_minimum")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double FeeMinimum { get; set; }

        [JsonProperty("wallet_fee_percent")]
        [JsonConverter(typeof(PriceToLongJsonConverter))]
        public long FeePercent { get; set; }

        [JsonProperty("wallet_publisher_fee_percent_default")]
        [JsonConverter(typeof(PriceToLongJsonConverter))]
        public long DefaultPublisherFeePercent { get; set; }

        [JsonProperty("wallet_balance")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double Balance { get; set; }

        [JsonProperty("wallet_trade_max_balance")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double TradeMaxBalance { get; set; }

        [JsonProperty("wallet_max_balance")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double MaxBalance { get; set; }
    }
}