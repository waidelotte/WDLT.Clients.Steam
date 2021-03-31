using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models
{
    public class ListingItem
    {
        public long Id { get; set; }
        public EApp App { get; set; }
        public double Price { get; set; }
        public string HashName { get; set; }
    }
}