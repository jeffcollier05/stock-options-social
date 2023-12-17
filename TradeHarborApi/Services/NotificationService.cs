using TradeHarborApi.Models.Notification;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    /// <summary>
    /// Service for handling notifications related to social interactions.
    /// </summary>
    public class NotificationService
    {
        private readonly SocialRepository _socialRepository;
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="socialRepository">Repository for social-related data operations.</param>
        /// <param name="authService">Service for handling authentication-related operations.</param>
        public NotificationService(
            SocialRepository socialRepository,
            AuthService authService
            )
        {
            _socialRepository = socialRepository;
            _authService = authService;
        }

        /// <summary>
        /// Notifies a user that their friend request has been accepted.
        /// </summary>
        /// <param name="requesterUserId">The user ID of the friend request sender.</param>
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

        /// <summary>
        /// Notifies a user that a comment has been left on their post.
        /// </summary>
        /// <param name="postOwnerUserId">The user ID of the post owner.</param>
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
