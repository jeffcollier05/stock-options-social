using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models
{
    public class AcceptFriendRequestRequest
    {
        [Required(ErrorMessage = "The RequesterUserId field is required.")]
        public string RequesterUserId { get; set; } = string.Empty;
    }
}
