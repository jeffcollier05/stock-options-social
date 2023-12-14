using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.PostFeatures
{
    /// <summary>
    /// Represents a request object for submitting a comment on a post.
    /// </summary>
    public class PostCommentRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post to which the comment is submitted.
        /// </summary>
        [Required(ErrorMessage = "The PostId field is required.")]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the content of the comment to be submitted.
        /// </summary>
        [Required(ErrorMessage = "The Comment field is required.")]
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user ID of the owner of the post to which the comment is submitted.
        /// </summary>
        [Required(ErrorMessage = "The PostOwnerUserId field is required.")]
        public string PostOwnerUserId { get; set; } = string.Empty;
    }
}
