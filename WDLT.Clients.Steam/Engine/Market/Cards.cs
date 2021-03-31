using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using WDLT.Clients.Base;
using WDLT.Clients.Steam.Enums;
using WDLT.Clients.Steam.Models.Json;

namespace WDLT.Clients.Steam.Engine.Market
{
    public class Cards
    {
        private readonly SteamClient _client;

        public Cards(SteamClient client)
        {
            _client = client;
        }

        public async Task<List<JBooster>> AviableBoosters(Proxy proxy = null)
        {
            var resp = await _client.SteamRequestRawAsync(new RestRequest("/tradingcards/boostercreator"), true, proxy);

            var match = Regex.Match(resp, @"(?<=CBoosterCreatorPage.Init\()(.*)(?<=}])", RegexOptions.Singleline);

            return JsonConvert.DeserializeObject<List<JBooster>>(match.Value.Trim());
        }

        public Task<JGemPrice> GemPrice(EApp app, long itemType, int borderColor = 0, Proxy proxy = null)
        {
            return _client.SteamRequestAsync<JGemPrice>(new RestRequest($"/auction/ajaxgetgoovalueforitemtype/?appid={(int)app}&item_type={itemType}&border_color={borderColor}"), false, proxy);
        }
    }
}