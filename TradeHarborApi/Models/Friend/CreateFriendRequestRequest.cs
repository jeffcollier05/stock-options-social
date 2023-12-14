using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models
{
    public class CreateFriendRequestRequest
    {
        [Required(ErrorMessage = "The ReceiverUserId field is required.")]
        public string ReceiverUserId { get; set; } = string.Empty;
    }
}
