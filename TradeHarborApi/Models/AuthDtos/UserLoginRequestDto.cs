using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.AuthDtos
{
    /// <summary>
    /// Represents the data transfer object (DTO) for user login requests.
    /// </summary>
    public class UserLoginRequestDto
    {
        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password for authentication.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
