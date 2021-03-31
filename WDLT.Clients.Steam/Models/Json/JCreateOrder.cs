using Newtonsoft.Json;
using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JCreateOrder : JSuccess<EOrderStatus>
    {
        [JsonProperty("buy_orderid")]
        public long? BuyOrderId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}