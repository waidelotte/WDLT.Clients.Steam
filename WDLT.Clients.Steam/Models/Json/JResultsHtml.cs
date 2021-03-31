using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JResultsHtml : JSuccess<bool>
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("pagesize")]
        public int PageSize { get; set; }

        [JsonProperty("results_html")]
        public string ResultsHtml { get; set; }
    }
}