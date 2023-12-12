namespace TradeHarborApi.Models.PostFeatures
{
    public class PostCommentRequest
    {
        public string PostId { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public string PostOwnerUserId { get; set; } = string.Empty;
    }
}
