using System.ComponentModel.DataAnnotations;

namespace TradeHarborApi.Models.Notification
{
    public class DeleteNotificationRequest
    {
        [Required(ErrorMessage = "The NotificationId field is required.")]
        public string NotificationId { get; set; } = string.Empty;
    }
}
