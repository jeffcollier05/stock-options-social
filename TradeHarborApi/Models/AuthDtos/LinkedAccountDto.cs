namespace TradeHarborApi.Models.AuthDtos
{
    /// <summary>
    /// Represents linked account information for a user.
    /// </summary>
    public class LinkedAccountDto
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the user's profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
