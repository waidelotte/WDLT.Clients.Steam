using System.Collections.Generic;

namespace WDLT.Clients.Steam.Models
{
    public class MarketHistogram
    {
        public double? MinSellPrice { get; set; }
        public double? HighBuyOrder { get; set; }

        public List<MarketHistogramItem> SellOrderGraph { get; set; }
        public List<MarketHistogramItem> BuyOrderGraph { get; set; }
    }
}