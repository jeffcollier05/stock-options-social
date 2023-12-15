using TradeHarborApi.Models;
using TradeHarborApi.Models.Notification;
using TradeHarborApi.Models.Post;
using TradeHarborApi.Models.PostFeatures;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    /// <summary>
    /// Service responsible for handling social-related actions such as trade posts, user interactions, and notifications.
    /// </summary>
    public class SocialService
    {
        private readonly SocialRepository _socialRepository;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialService"/> class.
        /// </summary>
        /// <param name="socialRepository">The repository for social-related data.</param>
        /// <param name="authService">The authentication service for user-related operations.</param>
        /// <param name="notificationService">The service for managing notifications.</param>
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

        /// <summary>
        /// Retrieves a collection of trade posts for the authenticated user, including comments and timestamps.
        /// </summary>
        /// <returns>An asynchronous task that represents the operation. The task result is a collection of <see cref="TradePost"/>.</returns>
        internal async Task<IEnumerable<TradePost>> GetTrades()
        {
            var userId = _authService.GetUserIdFromJwt();
            var tradePosts = await _socialRepository.GetTrades(userId);

             // Use the connection worker pool to gather comments for posts
            await Task.WhenAll(tradePosts.Select(async post =>
            {
                post.Comments = await _socialRepository.GetCommentsForPost(post.TradeId);
                post.Timestamp = DateTime.SpecifyKind(post.Timestamp, DateTimeKind.Utc);
            }));

            return tradePosts;
        }

        /// <summary>
        /// Creates a new trade post based on the provided request and the authenticated user.
        /// </summary>
        /// <param name="request">The request containing information for creating a trade post.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task CreateTradePost(CreateTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.CreateTradePost(request, userId);
        }

        /// <summary>
        /// Deletes a trade post based on the provided request and the authenticated user.
        /// </summary>
        /// <param name="request">The request containing information for deleting a trade post.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task DeleteTradePost(DeleteTradePostRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.DeleteTradePost(request, userId);
        }

        /// <summary>
        /// Retrieves a collection of user profiles, excluding the authenticated user.
        /// </summary>
        /// <returns>An asynchronous task that represents the operation. The task result is a collection of <see cref="UserProfile"/>.</returns>
        internal async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var userId = _authService.GetUserIdFromJwt();
            var users = await _socialRepository.GetAllUsers(userId);
            return users;
        }

        /// <summary>
        /// Removes a friend connection between the authenticated user and the specified friend.
        /// </summary>
        /// <param name="request">The request containing information for removing a friend connection.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task RemoveFriend(ModifyFriendPairRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository .RemoveFriend(request.FriendUserId, userId);
        }

        /// <summary>
        /// Retrieves a collection of notifications for the authenticated user.
        /// </summary>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is a collection of <see cref="Notification"/>.
        /// </returns>
        internal async Task<IEnumerable<Notification>> GetNotifications()
        {
            var userId = _authService.GetUserIdFromJwt();
            var notifications = await _socialRepository.GetNotifications(userId);
            return notifications;
        }

        /// <summary>
        /// Deletes a notification based on the provided request and the authenticated user.
        /// </summary>
        /// <param name="request">The request containing information for deleting a notification.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task DeleteNotification(DeleteNotificationRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.DeleteNotification(request, userId);
        }

        /// <summary>
        /// Creates a friend request based on the provided request and the authenticated user.
        /// </summary>
        /// <param name="request">The request containing information for creating a friend request.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task CreateFriendRequest(CreateFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.CreateFriendRequest(userId, request);
        }

        /// <summary>
        /// Accepts a friend request based on the provided request and the authenticated user.
        /// Notifies the requester that the user accepted their friend request.
        /// </summary>
        /// <param name="request">The request containing information for accepting a friend request.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.AcceptFriendRequest(request.RequesterUserId, userId);

            // Notify requester that the user accepted their friend request
            await _notificationService.NotifyFriendRequestAccpeted(request.RequesterUserId);
        }

        /// <summary>
        /// Declines a friend request based on the provided request and the authenticated user.
        /// </summary>
        /// <param name="request">The request containing information for declining a friend request.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.DeclineFriendRequest(request.RequesterUserId, userId);
        }

        /// <summary>
        /// Reacts to a post based on the provided request and the authenticated user.
        /// </summary>
        /// <param name="request">The request containing information for reacting to a post.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task ReactToPost(PostReactionRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.ReactToPost(request, userId);
        }

        /// <summary>
        /// Comments on a post based on the provided request and the authenticated user, notifying the post owner.
        /// </summary>
        /// <param name="request">The request containing information for commenting on a post.</param>
        /// <returns>An asynchronous task that represents the operation.</returns>
        internal async Task CommentOnPost(PostCommentRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _socialRepository.CommentOnPost(request, userId);

            // Alert post owner that comment was left
            await _notificationService.NotifyCommentOnPost(request.PostOwnerUserId);
        }
    }
}
