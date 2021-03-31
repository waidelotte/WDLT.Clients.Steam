using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using WDLT.Clients.Base;
using WDLT.Clients.Steam.Engine;
using WDLT.Clients.Steam.Engine.Market;

namespace WDLT.Clients.Steam
{
    public class SteamClient : BaseClient
    {
        public Auth Auth { get; }
        public Market Market { get; }
        public Cards Cards { get; }

        public SteamClient(string userAgent) : base(SteamEndpoints.BASE_HOST_URL, userAgent)
        {
            Auth = new Auth(this);
            Market = new Market(this);
            Cards = new Cards(this);
        }

        protected override void OnBeforeRequest(RestClient client, IRestRequest request, Proxy proxy = null)
        {
            client.Timeout = 15000;
            client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
        }

        public async Task<T> SteamRequestAsync<T>(IRestRequest request, bool withAuth, Proxy proxy = null)
        {
            var response = await SteamRequestRawAsync(request, withAuth, proxy);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<string> SteamRequestRawAsync(IRestRequest request, bool withAuth, Proxy proxy = null)
        {
            if (withAuth && Auth.CookieCollection != null)
            {
                foreach (Cookie cookie in Auth.CookieCollection)
                {
                    request.AddCookie(cookie.Name, cookie.Value);
                }
            }

            var client = proxy != null ? CreateClient(SteamEndpoints.BASE_HOST_URL, DefaultUserAgent) : _client;

            var response = await RequestRawAsync(client, request, proxy);

            if(withAuth) Auth.SetCookies(response.Cookies);

            return response.Content;
        }
    }
}
