using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using WDLT.Clients.Base;
using WDLT.Clients.Steam.Enums;
using WDLT.Clients.Steam.Exceptions;
using WDLT.Clients.Steam.Models;
using WDLT.Clients.Steam.Models.Json;

namespace WDLT.Clients.Steam.Engine.Market
{
    public class Market
    {
        private readonly SteamClient _client;

        public Market(SteamClient client)
        {
            _client = client;
        }

        public Task<JResultsNoRender> SearchAsync(string query = "", int start = 0, int count = 10,
            bool searchInDescriptions = false,
            EApp app = EApp.None, EMarketSort? columnSort = null, ESort? sort = null,
            IDictionary<string, string> custom = null, bool withAuth = false, Proxy proxy = null)
        {
            var request = new RestRequest("/market/search/render/");
            request.AddQueryParameter("query", query);
            request.AddQueryParameter("start", start.ToString());
            request.AddQueryParameter("count", count.ToString());
            request.AddQueryParameter("search_descriptions", (searchInDescriptions ? 1 : 0).ToString());
            request.AddQueryParameter("appid", ((int)app).ToString());
            if(columnSort != null) request.AddQueryParameter("sort_column", columnSort.Value.ToString().ToLower());
            if(sort != null) request.AddQueryParameter("sort_dir", sort.Value.ToString().ToLower());
            request.AddQueryParameter("norender", "1");

            if (custom != null)
            {
                foreach (var customQuery in custom)
                {
                    request.AddQueryParameter(customQuery.Key, customQuery.Value);
                }
            }

            return _client.SteamRequestAsync<JResultsNoRender>(request, withAuth, proxy);
        }

        public async Task<UserProfile> ProfileAsync(Proxy proxy = null)
        {
            var resp = await _client.SteamRequestRawAsync(new RestRequest("/market"), true, proxy);

            var doc = new HtmlDocument();
            doc.LoadHtml(resp);

            var userLogin = doc
                .DocumentNode
                .SelectSingleNode("//span[@id='market_buynow_dialog_myaccountname']")
                .InnerText;

            long.TryParse(Regex.Match(resp, "(?<=g_steamID = \")(.*)(?=\";)").Value, out var userId);

            var userAvatar = doc
                .DocumentNode
                .SelectSingleNode("//span[@class='avatarIcon']")
                .ChildNodes[1]
                .Attributes["src"]
                .Value;

            var walletInfoMatch = Regex.Match(resp, "(?<=g_rgWalletInfo = )(.*)(?=;)").Value;
            var walletInfoDes = JsonConvert.DeserializeObject<JWallet>(walletInfoMatch);

            if (walletInfoDes == null)
                throw new SteamException("Cannot load User Wallet");

            return new UserProfile
            {
                Avatar = userAvatar,
                Id = userId,
                Login = userLogin,
                Wallet = new UserWallet
                {
                    Currency = walletInfoDes.Currency,
                    Country = walletInfoDes.Country,
                    Balance = walletInfoDes.Balance,
                    MaxBalance = (long)walletInfoDes.MaxBalance,
                    Fee = walletInfoDes.Fee,
                    MinimumFee = walletInfoDes.FeeMinimum,
                    FeePercent = (int)walletInfoDes.FeePercent,
                    DefaultPublisherFeePercent = walletInfoDes.DefaultPublisherFeePercent,
                    TradeMaxBalance = (long)walletInfoDes.TradeMaxBalance
                }
            };
        }

        public Task<JListings> Listings(EApp app, string hashName, ECurrency currency, int count = 10, bool withAuth = false, Proxy proxy = null)
        {
            var request = new RestRequest($"/market/listings/{(int)app}/{hashName}/render");
            request.AddQueryParameter("currency", ((int)currency).ToString());
            request.AddQueryParameter("count", count.ToString());

            return _client.SteamRequestAsync<JListings>(request, withAuth, proxy);
        }

        public async Task<long?> SpreadID(EApp app, string hashName, Proxy proxy = null)
        {
            var resp = await _client.SteamRequestRawAsync(new RestRequest($"/market/listings/{(int)app}/{hashName}"), false, proxy);
            var doc = new HtmlDocument();
            doc.LoadHtml(resp);

            var spreadMatch = Regex.Match(resp, @"(?<=Market_LoadOrderSpread\()(.*)(?=\);)").Value.Trim();
            if(string.IsNullOrWhiteSpace(spreadMatch))
                throw new SteamException("Spread ID not Found");

            return long.Parse(spreadMatch);
        }

        public async Task<MarketHistogram> ItemHistogramAsync(long spreadID, string country, ELanguage lang, ECurrency currency, Proxy proxy = null)
        {
            var request = new RestRequest($"/market/itemordershistogram?country={country}&language={lang.ToString().ToLower()}&currency={(int)currency}&item_nameid={spreadID}");
            var resp = await _client.SteamRequestAsync<JItemOrderHistogram>(request, false, proxy);

            var histogram = new MarketHistogram();

            if (resp.SellOrderGraph != null) histogram.SellOrderGraph = ConvertOrderGraph(resp.SellOrderGraph);
            if (resp.BuyOrderGraph != null) histogram.BuyOrderGraph = ConvertOrderGraph(resp.BuyOrderGraph);

            histogram.MinSellPrice = resp.MinSellPrice;
            histogram.HighBuyOrder = resp.HighBuyOrder;

            return histogram;
        }

        private static List<MarketHistogramItem> ConvertOrderGraph(IReadOnlyCollection<dynamic[]> collection)
        {
            var graph = new List<MarketHistogramItem>();

            if (collection == null || collection.Count == 0)
                return graph;

            long tempCount = 0;
            var list = collection.Select(x =>
            {
                var item = new MarketHistogramItem
                {
                    Count = (int)x[1] - tempCount,
                    Price = (double)x[0],
                    Title = (string)x[2]
                };
                tempCount = item.Count;
                return item;
            }).ToList();

            graph.AddRange(list);

            return graph;
        }

        public async Task<List<MarketHistoryItem>> ItemHistoryAsync(EApp app, string hashName, Proxy proxy = null)
        {
            var request = new RestRequest("/market/pricehistory/");
            request.AddQueryParameter("appid", ((int) app).ToString());
            request.AddQueryParameter("market_hash_name", hashName, false);

            var resp = await _client.SteamRequestAsync<JPriceHistory>(request, true, proxy);

            return resp.Prices
                .Select((g, i) => new MarketHistoryItem
                {
                    Timestamp = DateTimeOffset.ParseExact(g[0].ToString(), "MMM dd yyyy HH: +0", CultureInfo.InvariantCulture),
                    Count = int.Parse(g[2].ToString()),
                    Price = Math.Round(double.Parse(g[1].ToString()), 2)
                })
                .OrderBy(o => o.Timestamp)
                .ToList();
        }

        public Task<JCreateOrder> CreateOrder(string hashName, EApp app, ECurrency currency, double totalPrice, int quantity, Proxy proxy = null)
        {
            var request = new RestRequest("/market/createbuyorder/", Method.POST);

            request.AddHeader("Referer", $"https://steamcommunity.com/market/listings/{(int)app}/{hashName}");
            request.AddParameter("sessionid", _client.Auth.Session(), ParameterType.GetOrPost);
            request.AddParameter("currency", (int)currency, ParameterType.GetOrPost);
            request.AddParameter("appid", (int)app, ParameterType.GetOrPost);
            request.AddParameter("market_hash_name", HttpUtility.UrlDecode(hashName), ParameterType.GetOrPost);
            request.AddParameter("price_total", (totalPrice * 100 * quantity).ToString(CultureInfo.InvariantCulture), ParameterType.GetOrPost);
            request.AddParameter("quantity", quantity, ParameterType.GetOrPost);

            return _client.SteamRequestAsync<JCreateOrder>(request, true, proxy);
        }

        public Task<JSuccess<EOrderStatus>> CancelOrder(long orderId, Proxy proxy = null)
        {
            var request = new RestRequest("/market/cancelbuyorder/", Method.POST);
            request.AddHeader("Referer", "https://steamcommunity.com/market/");
            request.AddParameter("sessionid", _client.Auth.Session(), ParameterType.GetOrPost);
            request.AddParameter("buy_orderid", orderId, ParameterType.GetOrPost);

            return _client.SteamRequestAsync<JSuccess<EOrderStatus>>(request, true, proxy);
        }

        public Task<JRecent> Recent(string country, ELanguage language, ECurrency currency, Proxy proxy = null)
        {
            var request = new RestRequest("/market/recent");

            request.AddQueryParameter("country", country);
            request.AddQueryParameter("language", language.ToString().ToLower());
            request.AddQueryParameter("currency", ((int)currency).ToString());

            return _client.SteamRequestAsync<JRecent>(request, false, proxy);
        }

        public Task<JPriceOverview> PriceOverview(ECurrency currency, EApp app, string hashName, Proxy proxy = null)
        {
            var request = new RestRequest("/market/priceoverview/");
            request.AddQueryParameter("currency", ((int)currency).ToString());
            request.AddQueryParameter("appid", ((int)app).ToString());
            request.AddQueryParameter("market_hash_name", hashName);

            return _client.SteamRequestAsync<JPriceOverview>(request, false, proxy);
        }

        public Task<string> BuyListing(long id, ECurrency currency, double subtotal, double total, Proxy proxy = null)
        {
            var subtotalConverted = (long)(subtotal * 100);
            var totalConverted = (long)(total * 100);
            var fee = totalConverted - subtotalConverted;
            return BuyListing(id, currency, subtotalConverted, fee, totalConverted, proxy);
        }

        public Task<string> BuyListing(long id, ECurrency currency, long subtotal, long fee, long total, Proxy proxy = null)
        {
            var request = new RestRequest($"/market/buylisting/{id}", Method.POST);
            request.AddHeader("Referer", "https://steamcommunity.com/market/");
            request.AddParameter("sessionid", _client.Auth.Session(), ParameterType.GetOrPost);
            request.AddParameter("currency", (int)currency, ParameterType.GetOrPost);
            request.AddParameter("quantity", 1, ParameterType.GetOrPost);
            request.AddParameter("subtotal", subtotal, ParameterType.GetOrPost);
            request.AddParameter("fee", fee, ParameterType.GetOrPost);
            request.AddParameter("total", total, ParameterType.GetOrPost);

            return _client.SteamRequestRawAsync(request, true, proxy);
        }

        public async Task<Listings> Listings(Proxy proxy = null)
        {
            var resp = await _client.SteamRequestAsync<JResultsHtml>(new RestRequest(SteamEndpoints.MARKET + "/market/mylistings/"), true, proxy);
            var linstings = new Listings();

            var doc = new HtmlDocument();
            doc.LoadHtml(resp.ResultsHtml);
            var root = doc.DocumentNode;

            var ordersCountNode = root
                .SelectSingleNode(".//span[@id='my_market_buylistings_number']")
                .InnerText;
            linstings.OrdersCount = int.Parse(ordersCountNode);

            var sellCountNode = root
                .SelectSingleNode(".//span[@id='my_market_selllistings_number']")
                .InnerText;
            linstings.OnSellCount = int.Parse(sellCountNode);

            if (linstings.OrdersCount > 0)
            {
                var ordersNodes = root.SelectNodes("//div[contains(@id,'mybuyorder_')]");

                foreach (var item in ordersNodes)
                {
                    var price = item.SelectSingleNode(".//span[@class='market_listing_price']").GetDirectInnerText().Trim().Split(' ').First().ParseSteamCurrency();

                    var orderId = long.Parse(Regex.Match(item.InnerHtml, "(?<=mbuyorder_)([0-9]*)(?=_name)").Value);

                    var url = item
                        .SelectSingleNode(".//a[@class='market_listing_item_name_link']")
                        .Attributes["href"].Value;

                    var hashName = url.HashNameFromUrl();
                    var app = url.AppFromUrl();

                    linstings.Orders.Add(new ListingItem
                    {
                        App = app,
                        HashName = hashName,
                        Id = orderId,
                        Price = price
                    });
                }
            }

            return linstings;
        }
    }
}