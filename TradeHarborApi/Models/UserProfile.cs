namespace TradeHarborApi.Models
{
    public class UserProfile
    {
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;

        public bool SentFriendRequestToYou { get; set; } = false;

        public bool ReceivedFriendRequestFromYou { get; set; } = false;

        public bool IsFriend { get; set; } = false;
    }
}
