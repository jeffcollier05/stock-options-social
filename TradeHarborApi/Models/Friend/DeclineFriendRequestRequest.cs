using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models
{
    /// <summary>
    /// Represents a request to decline a friend request.
    /// </summary>
    public class DeclineFriendRequestRequest
    {
        /// <summary>
        /// Gets or sets the user ID of the requester.
        /// </summary>
        [Required(ErrorMessage = "The RequesterUserId field is required.")]
        public string RequesterUserId { get; set; } = string.Empty;
    }
}
