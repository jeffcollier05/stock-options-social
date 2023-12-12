using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models
{
    public class ModifyFriendPairRequest
    {
        [Required(ErrorMessage = "The FriendUserId field is required.")]
        public string FriendUserId { get; set; } = string.Empty;
    }
}
