using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models
{
    /// <summary>
    /// Represents a request to modify a friend pair.
    /// </summary>
    public class ModifyFriendPairRequest
    {
        /// <summary>
        /// Gets or sets the user ID of the friend.
        /// </summary>
        [Required(ErrorMessage = "The FriendUserId field is required.")]
        public string FriendUserId { get; set; } = string.Empty;
    }
}
