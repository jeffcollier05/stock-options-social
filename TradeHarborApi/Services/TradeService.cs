using TradeHarborApi.Models;
using TradeHarborApi.Models.Notification;
using TradeHarborApi.Models.Post;
using TradeHarborApi.Models.PostFeatures;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class TradeService
    {
        private readonly TradesRepository _tradesRepo;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;

        public TradeService(
            TradesRepository tradesRepo,
            AuthService authService,
            NotificationService notificationService)
        {
            _tradesRepo = tradesRepo;
            _authService = authService;
            _notificationService = notificationService;
        }

        internal async Task<IEnumerable<TradePost>> GetTrades()
        {
            var userId = _authService.GetUserIdFromJwt();
            var tradePosts = await _tradesRepo.GetTrades(userId);
            foreach (var post in tradePosts)
            {
                post.Comments = await _tradesRepo.GetCommentsForPost(post.TradeId);
                post.Timestamp = DateTime.SpecifyKind(post.Timestamp, DateTimeKind.Utc);
            }

            return tradePosts;
        }

        internal async Task CreateTradePost(CreateTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.CreateTradePost(request, userId);
        }

        internal async Task DeleteTradePost(DeleteTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.DeleteTradePost(request, userId);
        }

        internal async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var userId = _authService.GetUserIdFromJwt();
            var users = await _tradesRepo.GetAllUsers(userId);
            return users;
        }

        internal async Task RemoveFriend(ModifyFriendPairRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.RemoveFriend(request.FriendUserId, userId);
        }

        internal async Task<IEnumerable<Notification>> GetNotifications()
        {
            var userId = _authService.GetUserIdFromJwt();
            var notifications = await _tradesRepo.GetNotifications(userId);
            return notifications;
        }

        internal async Task DeleteNotification(DeleteNotificationRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.DeleteNotification(request, userId);
        }

        internal async Task CreateFriendRequest(CreateFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.CreateFriendRequest(userId, request, DateTime.UtcNow);
        }

        internal async Task AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.AcceptFriendRequest(request.RequesterUserId, userId);

            // Notify requester that the user accepted their friend request
            await _notificationService.NotifyFriendRequestAccpeted(request.RequesterUserId);
        }

        internal async Task DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.DeclineFriendRequest(request.RequesterUserId, userId);
        }

        internal async Task ReactToPost(PostReactionRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.ReactToPost(request, userId);
        }

        internal async Task CommentOnPost(PostCommentRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.CommentOnPost(request, userId);

            // Alert post owner that comment was left
            await _notificationService.NotifyCommentOnPost(request.PostOwnerUserId);
        }
    }
}
