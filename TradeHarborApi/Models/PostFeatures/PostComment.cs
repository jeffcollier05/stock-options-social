namespace TradeHarborApi.Models.PostFeatures
{
    public class PostComment
    {
        public string CommentId { get; set; } = string.Empty;

        public string PostId { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public string Username { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
