namespace TradeHarborApi.Models
{
    /// <summary>
    /// Represents a user profile in relation to the user with JWT.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the profile picture URL.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether a friend request was sent to the user.
        /// </summary>
        public bool SentFriendRequestToYou { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a friend request was received from the user.
        /// </summary>
        public bool ReceivedFriendRequestFromYou { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the user is a friend.
        /// </summary>
        public bool IsFriend { get; set; } = false;
    }
}
