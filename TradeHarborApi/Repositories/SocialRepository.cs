using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Azure.Core;
using Dapper;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models;
using TradeHarborApi.Models.Notification;
using TradeHarborApi.Models.Post;
using TradeHarborApi.Models.PostFeatures;

namespace TradeHarborApi.Repositories
{
    /// <summary>
    /// Repository for handling data operations related to social interactions.
    /// </summary>
    public class SocialRepository
    {
        private readonly IApiConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRepository"/> class.
        /// </summary>
        /// <param name="config">API configuration.</param>
        public SocialRepository(IApiConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets a new open SqlConnection using the configured connection string.
        /// </summary>
        /// <returns>A new instance of <see cref="DbConnection"/> representing an open SqlConnection.</returns>
        private DbConnection GetSqlConnection()
        {
            var connection = new SqlConnection(_config.SqlConnectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Retrieves trade posts for a given user and their friends.
        /// </summary>
        /// <param name="userId">User ID for whom to retrieve trade posts.</param>
        /// <returns>Collection of trade posts.</returns>
        public async Task<IEnumerable<TradePost>> GetTrades(string userId)
        {
            var parameters = new
            {
                UserId = userId
            };

            var procedure = "dbo.GetTrades";
            using var connection = GetSqlConnection();
            var tradePosts = await connection.QueryAsync<TradePost>(procedure, parameters, commandType: CommandType.StoredProcedure);
            return tradePosts;
        }

        /// <summary>
        /// Deletes a trade post.
        /// </summary>
        /// <param name="request">Request object containing trade post details.</param>
        /// <param name="userId">User ID making the request.</param>
        public async Task DeleteTradePost(DeleteTradePostRequest request, string userId)
        {
            var parameters = new
            {
                UserId = userId,
                TradeId = request.TradeId
            };

            var procedure = "dbo.DeleteTradePost";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Creates a new trade post.
        /// </summary>
        /// <param name="request">Request object containing trade post details.</param>
        /// <param name="userId">User ID creating the trade post.</param>
        /// <returns>Object representing the newly created trade post.</returns>
        public async Task<int> CreateTradePost(CreateTradePostRequest request, string userId)
        {
            var parameters = new
            {
                UserId = userId,
                Ticker = request.Ticker,
                Position = request.Position,
                Option = request.Option,
                Strikeprice = request.Strikeprice,
                Comment = request.Comment,
                Timestamp = DateTime.UtcNow
            };

            var procedure = "dbo.CreateTradePost";
            using var connection = GetSqlConnection();
            var primaryKey = await connection.QueryAsync<int>(procedure, parameters, commandType: CommandType.StoredProcedure);
            return primaryKey.Single();
        }

        /// <summary>
        /// Retrieves user profiles for all users except the current user.
        /// </summary>
        /// <param name="userId">User ID for whom to exclude from the results.</param>
        /// <returns>Collection of user profiles.</returns>
        public async Task<IEnumerable<UserProfile>> GetAllUsers(string userId)
        {
            var parameters = new
            {
                UserId = userId
            };

            var procedure = "dbo.GetAllUsers";
            using var connection = GetSqlConnection();
            var users = await connection.QueryAsync<UserProfile>(procedure, parameters, commandType: CommandType.StoredProcedure);
            return users;
        }

        /// <summary>
        /// Removes a friend connection between two users.
        /// </summary>
        /// <param name="user1Id">User ID of the first user.</param>
        /// <param name="user2Id">User ID of the second user.</param>
        public async Task RemoveFriend(string user1Id, string user2Id)
        {
            var parameters = new
            {
                User1Id = user1Id,
                User2Id = user2Id
            };

            var procedure = "dbo.RemoveFriend";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Creates a notification for a user.
        /// </summary>
        /// <param name="request">Request object containing notification details.</param>
        public async Task CreateNotification(CreateNotificationRequest request)
        {
            var parameters = new
            {
                UserId = request.UserId,
                Message = request.Message,
                CreatedTimestamp = request.CreatedTimestamp
            };

            var procedure = "dbo.CreateNotification";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <param name="userId">User ID for whom to retrieve notifications.</param>
        /// <returns>Collection of notifications.</returns>
        public async Task<IEnumerable<Notification>> GetNotifications(string userId)
        {
            var parameters = new
            {
                UserId = userId
            };

            var procedure = "dbo.GetNotifications";
            using var connection = GetSqlConnection();
            var notifications = await connection.QueryAsync<Notification>(procedure, parameters, commandType: CommandType.StoredProcedure);
            return notifications;
        }

        /// <summary>
        /// Deletes a notification for a specific user.
        /// </summary>
        /// <param name="request">Request object containing notification details.</param>
        /// <param name="userId">User ID for whom to delete the notification.</param>
        public async Task DeleteNotification(DeleteNotificationRequest request, string userId)
        {
            var parameters = new
            {
                UserId = userId,
                NotificationId = request.NotificationId
            };

            var procedure = "dbo.DeleteNotification";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Creates a friend request between two users.
        /// </summary>
        /// <param name="requesterUserId">User ID of the requester.</param>
        /// <param name="request">Request object containing friend request details.</param>
        public async Task CreateFriendRequest(string requesterUserId, CreateFriendRequestRequest request)
        {
            var parameters = new
            {
                SentTimestamp = DateTime.UtcNow,
                RequesterUserId = requesterUserId,
                ReceiverUserId = request.ReceiverUserId,
            };

            var procedure = "dbo.CreateFriendRequest";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Declines a friend request from a specific user.
        /// </summary>
        /// <param name="requesterUserId">User ID of the requester.</param>
        /// <param name="receiverUserId">User ID of the receiver.</param>
        public async Task DeclineFriendRequest(string requesterUserId, string receiverUserId)
        {
            var parameters = new
            {
                RequesterUserId = requesterUserId,
                ReceiverUserId = receiverUserId,
            };

            var procedure = "dbo.DeclineFriendRequest";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Accepts a friend request and establishes a friend connection between two users.
        /// </summary>
        /// <param name="requesterUserId">User ID of the requester.</param>
        /// <param name="receiverUserId">User ID of the receiver.</param>
        public async Task AcceptFriendRequest(string requesterUserId, string receiverUserId)
        {
            var parameters = new
            {
                RequesterUserId = requesterUserId,
                ReceiverUserId = receiverUserId,
            };

            var procedure = "dbo.AcceptFriendRequest";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Reacts to a post by adding or updating a reaction from the user.
        /// </summary>
        /// <param name="request">Request object containing post reaction details.</param>
        /// <param name="userId">User ID of the reacting user.</param>
        public async Task ReactToPost(PostReactionRequest request, string userId)
        {
            var parameters = new
            {
                UserId = userId,
                ReactionType = request.ReactionType,
                PostId = request.PostId,
                Timestamp = DateTime.UtcNow
            };

            var procedure = "dbo.ReactToPost";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Adds a comment on a specific post.
        /// </summary>
        /// <param name="request">Request object containing post comment details.</param>
        /// <param name="userId">User ID of the commenting user.</param>
        public async Task CommentOnPost(PostCommentRequest request, string userId)
        {
            var parameters = new
            {
                UserId = userId,
                Comment = request.Comment,
                PostId = request.PostId,
                Timestamp = DateTime.UtcNow
            };

            var procedure = "dbo.CommentOnPost";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Retrieves comments for a specific post.
        /// </summary>
        /// <param name="postId">ID of the post to retrieve comments for.</param>
        /// <returns>An enumerable collection of post comments.</returns>
        public async Task<IEnumerable<PostComment>> GetCommentsForPost(string postId)
        {
            var parameters = new
            {
                PostId = postId
            };

            var procedure = "dbo.GetCommentsForPost";
            using var connection = GetSqlConnection();
            var comments = await connection.QueryAsync<PostComment>(procedure, parameters, commandType: CommandType.StoredProcedure );
            return comments;
        }

        /// <summary>
        /// Retrieves user statistics based on the specified user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// An asynchronous task that represents the operation and returns a <see cref="UserStatistics"/>.
        /// </returns>
        public async Task<UserStatistics> GetUserStatistics(string userId)
        {
            var parameters = new
            {
                UserId = userId
            };

            var procedure = "dbo.GetUserStatistics";
            using var connection = GetSqlConnection();
            var statistics = await connection.QueryAsync<UserStatistics>(procedure, parameters, commandType: CommandType.StoredProcedure);
            return statistics.Single();
        }
    }
}
