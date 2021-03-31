using System.ComponentModel.DataAnnotations;

namespace WDLT.Clients.Steam.Enums
{
    public enum EOrderStatus
    {
        None = -1,
        Success = 1,
        [Display(Name = "Already Placed")]
        OrderAlreadyPlaced = 29,
    }
}