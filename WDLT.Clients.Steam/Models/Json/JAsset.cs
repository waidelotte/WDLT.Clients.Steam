using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JAsset
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("classid")]
        public long СlassId { get; set; }

        [JsonProperty("contextid")]
        public long ContextId { get; set; }

        [JsonProperty("appid")]
        public EApp App { get; set; }

        [JsonProperty("commodity")]
        public int Сommodity { get; set; }

        [JsonProperty("market_hash_name")]
        public string HashName { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("market_marketable_restriction")]
        public int? MarketableRestriction { get; set; }

        [JsonProperty("owner_actions")]
        public List<JAssetOwnerAction> OwnerActions { get; set; }

        public int? GooID()
        {
            var goo = OwnerActions?.FirstOrDefault(f => f.Link.Contains("GetGooValue"));

            if (goo != null)
            {
                return int.Parse(goo.Link.Split(",").ElementAt(3).Trim());
            }
            else
            {
                return null;
            }
        }
    }
}