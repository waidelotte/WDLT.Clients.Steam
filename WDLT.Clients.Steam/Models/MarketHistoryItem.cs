using System;

namespace WDLT.Clients.Steam.Models
{
    public class MarketHistoryItem
    {
        public DateTimeOffset Timestamp { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}