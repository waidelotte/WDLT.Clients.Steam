using System.Collections.Generic;

namespace WDLT.Clients.Steam.Models
{
    public class Listings
    {
        public int OnSellCount { get; set; }
        public int OrdersCount { get; set; }

        public List<ListingItem> Orders { get; set; } = new List<ListingItem>();
    }
}