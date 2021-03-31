namespace WDLT.Clients.Steam
{
    public static class SteamEndpoints
    {
        public const string BASE_HOST = "steamcommunity.com";
        public const string BASE_HOST_URL = "https://" + BASE_HOST;
        public const string LOGIN = BASE_HOST_URL + "/login";
        public const string LOGOUT = LOGIN + "/logout";
        public const string RSA = LOGIN + "/getrsakey";
        public const string DO_LOGIN = LOGIN + "/dologin";
        public const string MARKET = BASE_HOST_URL + "/market";
        public const string INVERTORY = BASE_HOST_URL + "/inventory/";
        public const string CDN_ECONOMY_IMAGE = "http://cdn.steamcommunity.com/economy/image/";
    }
}