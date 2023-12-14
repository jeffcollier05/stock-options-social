using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.Post
{
    /// <summary>
    /// Represents a request object for deleting a trade post.
    /// </summary>
    public class DeleteTradePostRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the trade post to be deleted.
        /// </summary>
        [Required(ErrorMessage = "The TradeId field is required.")]
        public string TradeId { get; set; } = string.Empty;
    }
}
