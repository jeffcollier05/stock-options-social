namespace TradeHarborApi.Models.Notification
{
    /// <summary>
    /// Represents a notification entity.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the notification was created.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the notification.
        /// </summary>
        public string NotificationId { get; set; } = string.Empty;
    }
}
