namespace TradeHarborApi.Models.Notification
{
    public class CreateNotificationRequest
    {
        public string UserId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedTimestamp { get; set; }
    }
}
