using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.AuthDtos
{
    /// <summary>
    /// Represents the data transfer object (DTO) for user registration requests.
    /// </summary>
    public class UserRegistrationRequestDto
    {
        /// <summary>
        /// Gets or sets the email address for the new user.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the new user.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the new user.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the new user.
        /// </summary>
        [Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username chosen by the new user.
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the profile picture chosen by the new user.
        /// </summary>
        [Required]
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
