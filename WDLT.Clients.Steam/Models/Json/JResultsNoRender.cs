using System.Collections.Generic;
using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JResultsNoRender : JSuccess<bool>
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("pagesize")]
        public int PageSize { get; set; }

        [JsonProperty("results")]
        public List<JSearchItem> Results { get; set; }
    }
}