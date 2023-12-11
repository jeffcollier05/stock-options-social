namespace TradeHarborApi.Models
{
    public class PostReactionRequest
    {
        public string PostId { get; set; } = string.Empty;

        public string ReactionType {  get; set; } = string.Empty;
    }
}
