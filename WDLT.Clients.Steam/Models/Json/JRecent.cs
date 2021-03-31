using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JRecent : JListingsBase
    {
        [JsonProperty("last_time")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset LastTime { get; set; }

        [JsonProperty("last_listing")]
        public string LastListing { get; set; }
    }
}