using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.PostFeatures
{
    /// <summary>
    /// Represents a request object for submitting a reaction to a post.
    /// </summary>
    public class PostReactionRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post to which the reaction is submitted.
        /// </summary>
        [Required(ErrorMessage = "The PostId field is required.")]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of reaction submitted to the post.
        /// </summary>
        [Required(ErrorMessage = "The ReactionType field is required.")]
        public string ReactionType { get; set; } = string.Empty;
    }
}
