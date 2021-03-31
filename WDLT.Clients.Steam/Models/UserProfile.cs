namespace WDLT.Clients.Steam.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Avatar { get; set; }

        public UserWallet Wallet { get; set; }
    }
}