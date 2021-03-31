using WDLT.Clients.Steam.Enums;

namespace WDLT.Clients.Steam.Models
{
    public class UserWallet
    {
        public ECurrency Currency { get; set; }

        public string Country { get; set; }

        public double Fee { get; set; }

        public double MinimumFee { get; set; }

        public int FeePercent { get; set; }

        public long DefaultPublisherFeePercent { get; set; }

        public double Balance { get; set; }

        public long MaxBalance { get; set; }

        public long TradeMaxBalance { get; set; }

        public double MinPriceForOrder()
        {
            return MinimumFee + Fee + Fee;
        }

        public double MinFee()
        {
            return MinimumFee + Fee;
        }

        public int MinFeeFlat()
        {
            return (int)((MinimumFee + Fee) * 100);
        }
    }
}