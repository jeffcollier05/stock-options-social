using TradeHarborApi.Models;
using TradeHarborApi.Models.Notification;
using TradeHarborApi.Models.Post;
using TradeHarborApi.Models.PostFeatures;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class SocialService
    {
        private readonly SocialRepository _socialRepository;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;

        public SocialService(
            SocialRepository socialRepository,
            AuthService authService,
            NotificationService notificationService
            )
        {
            _socialRepository = socialRepository;
            _authService = authService;
            _notificationService = notificationService;
        }

        internal async Task<IEnumerable<TradePost>> GetTrades()
        {
            var userId = _authService.GetUserIdFromJwt();
            var tradePosts = await _socialRepository.GetTrades(userId);

            // Get information for posts
            foreach (var post in tradePosts)
            {
                post.Comments = await _socialRepository.GetCommentsForPost(post.TradeId);

                // Mssql doesn't store timezone so this designates it
                post.Timestamp = DateTime.SpecifyKind(post.Timestamp, DateTimeKind.Utc);
            }

            return tradePosts;
        }

        internal async Task CreateTradePost(CreateTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.CreateTradePost(request, userId);
        }

        internal async Task DeleteTradePost(DeleteTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.DeleteTradePost(request, userId);
        }

        internal async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var userId = _authService.GetUserIdFromJwt();
            var users = await _socialRepository.GetAllUsers(userId);
            return users;
        }

        internal async Task RemoveFriend(ModifyFriendPairRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository .RemoveFriend(request.FriendUserId, userId);
        }

        internal async Task<IEnumerable<Notification>> GetNotifications()
        {
            var userId = _authService.GetUserIdFromJwt();
            var notifications = await _socialRepository.GetNotifications(userId);
            return notifications;
        }

        internal async Task DeleteNotification(DeleteNotificationRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.DeleteNotification(request, userId);
        }

        internal async Task CreateFriendRequest(CreateFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.CreateFriendRequest(userId, request);
        }

        internal async Task AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.AcceptFriendRequest(request.RequesterUserId, userId);

            // Notify requester that the user accepted their friend request
            await _notificationService.NotifyFriendRequestAccpeted(request.RequesterUserId);
        }

        internal async Task DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.DeclineFriendRequest(request.RequesterUserId, userId);
        }

        internal async Task ReactToPost(PostReactionRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.ReactToPost(request, userId);
        }

        internal async Task CommentOnPost(PostCommentRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.CommentOnPost(request, userId);

            // Alert post owner that comment was left
            await _notificationService.NotifyCommentOnPost(request.PostOwnerUserId);
        }
    }
}
