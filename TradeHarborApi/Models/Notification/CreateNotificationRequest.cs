namespace TradeHarborApi.Models.Notification
{
    /// <summary>
    /// Represents a request object for creating a notification.
    /// </summary>
    public class CreateNotificationRequest
    {
        /// <summary>
        /// Gets or sets the user ID associated with the notification.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the notification was created.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }
    }
}
