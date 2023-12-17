using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.Post
{
    /// <summary>
    /// Represents a request object for creating a trade post.
    /// </summary>
    public class CreateTradePostRequest
    {
        /// <summary>
        /// Gets or sets the ticker symbol for the trade post.
        /// </summary>
        [Required(ErrorMessage = "The Ticker field is required.")]
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the position for the trade post.
        /// </summary>
        [Required(ErrorMessage = "The Position field is required.")]
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the option for the trade post.
        /// </summary>
        [Required(ErrorMessage = "The Option field is required.")]
        public string Option { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strike price for the trade post.
        /// </summary>
        [Required(ErrorMessage = "The Strikeprice field is required.")]
        public string Strikeprice { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any additional comments or notes for the trade post.
        /// </summary>
        [Required(ErrorMessage = "The Comment field is required.")]
        public string Comment { get; set; } = string.Empty;
    }
}
