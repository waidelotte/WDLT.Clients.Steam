using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JRSA : JSuccess<bool>
    {
        [JsonProperty("publickey_mod")]
        public string Module { get; set; }

        [JsonProperty("publickey_exp")]
        public string Exponent { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("token_gid")]
        public string TokenGid { get; set; }
    }
}