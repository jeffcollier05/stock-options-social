using TradeHarborApi.Models.PostFeatures;

namespace TradeHarborApi.Models.Post
{
    /// <summary>
    /// Represents a trade post entity.
    /// </summary>
    public class TradePost
    {
        /// <summary>
        /// Gets or sets the user ID associated with the trade post.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the trade post.
        /// </summary>
        public string TradeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ticker symbol associated with the trade post.
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the position related to the trade post.
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the option related to the trade post.
        /// </summary>
        public string Option { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strike price associated with the trade post.
        /// </summary>
        public string Strikeprice { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any additional comments or notes related to the trade post.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the trade post was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user associated with the trade post.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user associated with the trade post.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the user associated with the trade post.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the user's profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of votes received on the trade post.
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// Gets or sets the user's reaction to the trade post.
        /// </summary>
        public string UserReaction { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of comments associated with the trade post.
        /// </summary>
        public IEnumerable<PostComment> Comments { get; set; } = [];
    }
}
