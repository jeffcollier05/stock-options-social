namespace TradeHarborApi.Models.PostFeatures
{
    /// <summary>
    /// Represents a comment entity associated with a post.
    /// </summary>
    public class PostComment
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public string CommentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the post to which the comment belongs.
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the comment was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the comment.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the user's profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the user who created the comment.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user who created the comment.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user ID of the user who created the comment.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
    }
}
