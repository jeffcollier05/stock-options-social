namespace TradeHarborApi.Models.Notification
{
    /// <summary>
    /// View model of a notification for the user.
    /// </summary>
    public class Notification
    {
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedTimestamp { get; set; }

        public string NotificationId { get; set; } = string.Empty;
    }
}
