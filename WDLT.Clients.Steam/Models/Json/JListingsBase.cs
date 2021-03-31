using System.Collections.Generic;
using Newtonsoft.Json;
using WDLT.Clients.Base;
using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models.Json
{
    public abstract class JListingsBase : JSuccess<bool>
    {
        [JsonProperty("listinginfo")]
        [JsonConverter(typeof(ArrayedObjectConverter))]
        public Dictionary<long, JListing> Listings { get; set; }

        [JsonProperty("assets")]
        [JsonConverter(typeof(ArrayedObjectConverter))]
        public Dictionary<EApp, Dictionary<long, Dictionary<long, JAsset>>> Assets { get; set; }
    }
}