using Azure.Core;
using TradeHarborApi.Models;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class TradeService
    {
        private readonly TradesRepository _tradesRepo;
        private readonly AuthService _authService;

        public TradeService(TradesRepository tradesRepo, AuthService authService)
        {
            _tradesRepo = tradesRepo;
            _authService = authService;
        }

        public async Task<IEnumerable<TradePost>> GetTrades()
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

        public async Task CreateTradePost(CreateTradePostRequest request)
        {
            request.UserId = _authService.GetUserIdFromJwt();
            request.Timestamp = DateTime.UtcNow;
            await _tradesRepo.CreateTradePost(request);
        }

        public async Task DeleteTradePost(DeleteTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.DeleteTradePost(request, userId);
        }

        public async Task<IEnumerable<FriendProfile>> GetFriendsForUser()
        {
            var userId = _authService.GetUserIdFromJwt();
            var friends = await _tradesRepo.GetFriendsForUser(userId);
            return friends;
        }

        public async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var userId = _authService.GetUserIdFromJwt();
            var users = await _tradesRepo.GetAllUsers(userId);
            return users;
        }

        //public async Task AddFriend(ModifyFriendPairRequest request)
        //{
        //    var userId = _authService.GetUserIdFromJwt();
        //    await _tradesRepo.AddFriend(request.FriendUserId, userId);

        //    //temp VV
        //    var notification = new CreateNotificationRequest
        //    {
        //        UserId = _authService.GetUserIdFromJwt(),
        //        Message = "Test notification 1.",
        //        CreatedTimestamp = DateTime.UtcNow
        //    };
        //    await _tradesRepo.CreateNotification(notification);
        //    // test code ^^^
        //}

        public async Task RemoveFriend(ModifyFriendPairRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.RemoveFriend(request.FriendUserId, userId);
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            var userId = _authService.GetUserIdFromJwt();
            var notifications = await _tradesRepo.GetNotifications(userId);
            return notifications;
        }

        public async Task DeleteNotification(DeleteNotificationRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.DeleteNotification(request, userId);
        }

        public async Task CreateFriendRequest(CreateFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.CreateFriendRequest(userId, request, DateTime.UtcNow);
        }

        public async Task AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.AcceptFriendRequest(request.RequesterUserId, userId);

            // Notify requester that the user accepted their friend request
            await NotifyFriendRequestAccpeted(request.RequesterUserId);
        }

        private async Task NotifyFriendRequestAccpeted(string requesterUserId)
        {
            var profile = _authService.GetUserProfileFromJwt();
            var notification = new CreateNotificationRequest
            {
                UserId = requesterUserId,
                Message = $"{profile.Username} has accepted your friend request.",
                CreatedTimestamp = DateTime.UtcNow
            };
            await _tradesRepo.CreateNotification(notification);
        }

        public async Task DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.DeclineFriendRequest(request.RequesterUserId, userId);
        }

        public async Task ReactToPost(PostReactionRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.ReactToPost(request, userId);
        }

        public async Task CommentOnPost(PostCommentRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.CommentOnPost(request, userId);

            // Alert post owner that comment was left
            await NotifyCommentOnPost(request.PostOwnerUserId);
        }

        private async Task NotifyCommentOnPost(string postOwnerUserId)
        {
            var profile = _authService.GetUserProfileFromJwt();
            var notification = new CreateNotificationRequest
            {
                UserId = postOwnerUserId,
                Message = $"{profile.Username} commented on your post.",
                CreatedTimestamp = DateTime.UtcNow
            };
            await _tradesRepo.CreateNotification(notification);
        }
    }
}
