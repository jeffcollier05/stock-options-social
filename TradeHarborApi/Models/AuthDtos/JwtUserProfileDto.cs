namespace TradeHarborApi.Models.AuthDtos
{
    public class JwtUserProfileDto
    {
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
