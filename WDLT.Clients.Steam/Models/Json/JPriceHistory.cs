using System.Collections.Generic;
using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JPriceHistory : JSuccess<bool>
    {
        [JsonProperty("prices")]
        public List<List<dynamic>> Prices { get; set; }
    }
}