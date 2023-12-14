using TradeHarborApi.Models.Notification;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class NotificationService
    {
        private readonly SocialRepository _socialRepository;
        private readonly AuthService _authService;

        public NotificationService(
            SocialRepository socialRepository,
            AuthService authService
            )
        {
            _socialRepository = socialRepository;
            _authService = authService;
        }

        internal async Task NotifyFriendRequestAccpeted(string requesterUserId)
        {
            var profile = _authService.GetUserProfileFromJwt();
            var notification = new CreateNotificationRequest
            {
                UserId = requesterUserId,
                Message = $"{profile.Username} has accepted your friend request.",
                CreatedTimestamp = DateTime.UtcNow
            };
            await _socialRepository.CreateNotification(notification);
        }

        internal async Task NotifyCommentOnPost(string postOwnerUserId)
        {
            var profile = _authService.GetUserProfileFromJwt();
            var notification = new CreateNotificationRequest
            {
                UserId = postOwnerUserId,
                Message = $"{profile.Username} commented on your post.",
                CreatedTimestamp = DateTime.UtcNow
            };
            await _socialRepository.CreateNotification(notification);
        }
    }
}
