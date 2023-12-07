using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.Dtos
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
