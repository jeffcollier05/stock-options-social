using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models
{
    /// <summary>
    /// Represents a request to create a friend request.
    /// </summary>
    public class CreateFriendRequestRequest
    {
        /// <summary>
        /// Gets or sets the user ID of the receiver.
        /// </summary>
        [Required(ErrorMessage = "The ReceiverUserId field is required.")]
        public string ReceiverUserId { get; set; } = string.Empty;
    }
}
