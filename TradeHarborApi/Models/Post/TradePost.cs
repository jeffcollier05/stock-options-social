using TradeHarborApi.Models.PostFeatures;

namespace TradeHarborApi.Models.Post
{
    public class TradePost
    {
        public string UserId { get; set; } = string.Empty;

        public string TradeId { get; set; } = string.Empty;

        public string Ticker { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Option { get; set; } = string.Empty;

        public string Strikeprice { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;

        public int Votes { get; set; }

        public string UserReaction { get; set; } = string.Empty;

        public IEnumerable<PostComment> Comments { get; set; } = new List<PostComment>();
    }
}
