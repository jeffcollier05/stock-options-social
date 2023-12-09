namespace TradeHarborApi.Models
{
    public class TradePost
    {
        public string UserId { get; set; }

        public string TradeId { get; set; }

        public string Ticker { get; set; }

        public string Position { get; set; }

        public string Option { get; set; }

        public string Strikeprice { get; set; }

        public string Comment { get; set; }

        public DateTime Timestamp { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}
