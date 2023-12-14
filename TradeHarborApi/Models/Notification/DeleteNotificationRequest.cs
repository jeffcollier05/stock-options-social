using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a request object for deleting a notification.
/// </summary>
namespace TradeHarborApi.Models.Notification
{
    /// <summary>
    /// Represents a request object for deleting a notification.
    /// </summary>
    public class DeleteNotificationRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the notification to be deleted.
        /// </summary>
        [Required(ErrorMessage = "The NotificationId field is required.")]
        public string NotificationId { get; set; } = string.Empty;
    }
}
