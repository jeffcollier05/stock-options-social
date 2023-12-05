namespace TradeHarborApi.Models
{
    public class CreateTradePostRequest
    {
        public string Id { get; set; } = string.Empty;

        public string Ticker { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Option { get; set; } = string.Empty;

        public string Strikeprice { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}
