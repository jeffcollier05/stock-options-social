using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.AuthDtos
{
    public class UserLoginRequestDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
