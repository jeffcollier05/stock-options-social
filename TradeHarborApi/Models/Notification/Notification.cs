namespace TradeHarborApi.Models.Notification
{
    public class Notification
    {
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedTimestamp { get; set; }

        public string NotificationId { get; set; } = string.Empty;
    }
}
