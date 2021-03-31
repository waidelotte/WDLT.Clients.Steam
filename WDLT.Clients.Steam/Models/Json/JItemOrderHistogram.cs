using System.Collections.Generic;
using Newtonsoft.Json;
using WDLT.Clients.Base;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JItemOrderHistogram
    {
        [JsonProperty("lowest_sell_order")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double? MinSellPrice { get; set; }

        [JsonProperty("highest_buy_order")]
        [JsonConverter(typeof(LongToDoublePriceJsonConverter))]
        public double? HighBuyOrder { get; set; }

        [JsonProperty("sell_order_graph")]
        public List<dynamic[]> SellOrderGraph { get; set; }

        [JsonProperty("buy_order_graph")]
        public List<dynamic[]> BuyOrderGraph { get; set; }
    }
}