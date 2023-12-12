using System.Data.Common;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models;

namespace TradeHarborApi.Repositories
{
    public class TradesRepository
    {
        private readonly IApiConfiguration _config;

        public TradesRepository(IApiConfiguration config)
        {
            _config = config;
        }

        private DbConnection GetSqlConnection()
        {
            var connection = new SqlConnection(_config.SqlConnectionString);
            connection.Open();
            return connection;
        }

        public async Task<IEnumerable<TradePost>> GetTrades(string userId)
        {
            var query = @"
                    WITH FriendIDs AS (
                        SELECT Person2Id AS 'Id'
                        FROM dbo.friendpairs
                        WHERE Person1Id = @UserId
                        UNION
                        SELECT Person1Id AS 'Id'
                        FROM dbo.friendpairs
                        WHERE Person2Id = @UserId
                        UNION
                        SELECT @UserId
                    )

                    SELECT 
                        t.Ticker,
                        p.Position,
                        op.[Option],
                        t.Strikeprice,
                        t.Comment,
                        t.[Timestamp],
                        t.User_id AS 'UserId',
                        t.Trade_id AS 'TradeId',
                        a.FirstName,
                        a.LastName,
                        a.ProfilePictureUrl,
                        u.UserName as Username,
	                    (SELECT COUNT(*) FROM dbo.PostReactions WHERE PostId = t.Trade_id AND ReactionType = 'UPVOTE')
		                    - (SELECT COUNT(*) FROM dbo.PostReactions WHERE PostId = t.Trade_id AND ReactionType = 'DOWNVOTE') AS Votes,
	                    (SELECT ReactionType FROM dbo.PostReactions WHERE PostId = t.Trade_id AND UserId = @UserId) AS 'UserReaction'
                    FROM dbo.trades t
                    JOIN reference.[Option] op on op.Option_id = t.[Option]
                    JOIN reference.[Position] p on p.Position_id = t.Position
                    JOIN dbo.Accounts a on a.User_id = t.User_id
                    JOIN dbo.AspNetUsers u on u.Id = t.User_id
                    WHERE
                        u.Id IN (SELECT Id FROM FriendIDs)
                        AND t.IsActive = 1
                    ";

            using var connection = GetSqlConnection();
            var tradePosts = await connection.QueryAsync<TradePost>(query, new { userId });
            return tradePosts;
        }

        public async Task DeleteTradePost(DeleteTradePostRequest request, string userId)
        {
            var query = @"
                    UPDATE dbo.Trades
                    SET IsActive = 0
                    WHERE Trade_id = @TradeId AND User_id = @UserId
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { request.TradeId, userId});
        }

        public async Task<object> CreateTradePost(CreateTradePostRequest request)
        {
            var query = @"
                    DECLARE @PositionId INT;
                    SET @PositionId = (select Position_id from reference.Position where Position = @Position);

                    DECLARE @OptionId INT;
                    SET @OptionId = (select Option_id from reference.[Option] where [Option] = @Option); 

                    INSERT INTO [dbo].[Trades]
                        ([User_id], [Ticker], [Position], [Option], [Strikeprice], [Comment], [Timestamp], [IsActive])
                    VALUES (@UserId, @Ticker, @PositionId, @OptionId, @Strikeprice, @Comment, @Timestamp, 1);

                    DECLARE @PrimaryKey INT;
                    SET @PrimaryKey = SCOPE_IDENTITY();

                    SELECT @PrimaryKey AS 'NewPrimaryKey';
                    ";

            using var connection = GetSqlConnection();
            var tradePosts = await connection.QueryAsync<object>(query, request);
            return tradePosts;
        }

        public async Task<IEnumerable<FriendProfile>> GetFriendsForUser(string userId)
        {
            var query = @"
                    WITH FriendIDs AS (
                        SELECT Person2Id AS 'Id'
                        FROM dbo.friendpairs
                        WHERE Person1Id = @UserId
                        UNION
                        SELECT Person1Id AS 'Id'
                        FROM dbo.friendpairs
                        WHERE Person2Id = @UserId
                    )

                    SELECT f.Id as 'UserId', a.FirstName, a.LastName, a.ProfilePictureUrl, u.UserName
                    FROM FriendIDs f
                    JOIN dbo.Accounts a ON a.User_id = f.Id
                    JOIN dbo.AspNetUsers u on u.Id = f.Id
                    ;";

            using var connection = GetSqlConnection();
            var friends = await connection.QueryAsync<FriendProfile>(query, new { userId });
            return friends;
        }

        public async Task<IEnumerable<UserProfile>> GetAllUsers(string userId)
        {
            var query = @"
                    SELECT 
	                     u.Id as 'UserId',
	                     u.UserName,
	                     ac.FirstName,
	                     ac.LastName,
	                     ac.ProfilePictureUrl,
	                     CASE
                            WHEN EXISTS (
                                SELECT 1 
                                FROM dbo.FriendPairs F 
                                WHERE f.Person1Id = u.Id AND F.Person2Id = @UserId
                                   OR F.Person2Id = u.Id AND F.Person1Id = @UserId
                            ) THEN 1
                            ELSE 0
                        END AS IsFriend,
	                    CASE
                            WHEN EXISTS (
                                SELECT 1 
                                FROM dbo.FriendRequests fr
                                WHERE fr.RequesterUserId = u.Id AND fr.ReceiverUserId = @UserId
                            ) THEN 1
                            ELSE 0
                        END AS SentFriendRequestToYou,
	                    CASE
                            WHEN EXISTS (
                                SELECT 1 
                                FROM dbo.FriendRequests fr
                                WHERE fr.ReceiverUserId = u.Id AND fr.RequesterUserId = @UserId
                            ) THEN 1
                            ELSE 0
                        END AS ReceivedFriendRequestFromYou
                     FROM dbo.AspNetUsers u
                     JOIN dbo.Accounts ac on ac.User_id = u.Id
                     WHERE u.Id <> @UserId;
                    ;";

            using var connection = GetSqlConnection();
            var users = await connection.QueryAsync<UserProfile>(query, new { userId });
            return users;
        }

        public async Task RemoveFriend(string user1Id, string user2Id)
        {
            var query = @"
                    DELETE
                    FROM dbo.FriendPairs
                    WHERE
                      (Person1Id = @User1Id AND Person2Id = @User2Id)
                      OR
                      (Person1Id = @User2Id AND Person2Id = @User1Id)
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { user1Id, user2Id });
        }

        public async Task AddFriend(string requesterUserId, string receiverUserId)
        {
            var query = @"
                    INSERT INTO [dbo].[FriendPairs]
                        ([Person1Id]
                        ,[Person2Id]
                        ,[PairingDate])
                     VALUES
                        (@RequesterUserId,
                        @ReceiverUserId,
                        GETDATE())
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { requesterUserId, receiverUserId });
        }

        public async Task CreateNotification(CreateNotificationRequest request)
        {
            var query = @"
                    INSERT INTO [dbo].[Notifications]
                        ([UserId]
                        ,[Message]
                        ,[CreatedTimestamp]
                        ,[IsActive])
                     VALUES
                        (@UserId,
                        @Message,
                        @CreatedTimestamp,
                        1)
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, request);
        }

        public async Task<IEnumerable<Notification>> GetNotifications(string userId)
        {
            var query = @"
                    SELECT Message, CreatedTimestamp, NotificationId
                    FROM dbo.Notifications
                    WHERE UserId = @UserId
                        AND IsActive = 1
                    ;";

            using var connection = GetSqlConnection();
            var notifications = await connection.QueryAsync<Notification>(query, new { userId });
            return notifications;
        }

        public async Task DeleteNotification(DeleteNotificationRequest request, string userId)
        {
            var query = @"
                    UPDATE dbo.Notifications
                    SET IsActive = 0
                    WHERE NotificationId = @NotificationId AND UserId = @UserId
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { request.NotificationId, userId });
        }

        public async Task CreateFriendRequest(string requesterUserId, CreateFriendRequestRequest request, DateTime sentTimestamp)
        {
            var query = @"
                    INSERT INTO [dbo].[FriendRequests]
                        ([RequesterUserId]
                        ,[ReceiverUserId]
                        ,[SentTimestamp])
                     VALUES
                        (@RequesterUserId,
                        @ReceiverUserId,
                        @SentTimestamp)
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { requesterUserId, request.ReceiverUserId, sentTimestamp });
        }

        public async Task DeclineFriendRequest(string requesterUserId, string receiverUserId)
        {
            var query = @"
                    DELETE
                    FROM dbo.FriendRequests
                    WHERE
                      RequesterUserId = @RequesterUserId AND ReceiverUserId = @ReceiverUserId
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { requesterUserId, receiverUserId });
        }

        public async Task AcceptFriendRequest(string requesterUserId, string receiverUserId)
        {
            var query = @"
                    DECLARE @FriendRequestId int;
                    SET @FriendRequestId = (
	                    select FriendRequestId
	                    FROM [TradeHarbor].[dbo].[FriendRequests]
	                    WHERE RequesterUserId = @RequesterUserId AND ReceiverUserId = @ReceiverUserId
                    );

                    IF @FriendRequestId IS NOT NULL
                        BEGIN
	                    INSERT INTO [dbo].[FriendPairs]
		                    ([Person1Id]
		                    ,[Person2Id]
		                    ,[PairingDate])
	                    VALUES
		                    (@RequesterUserId,
		                    @ReceiverUserId,
		                    GETDATE());

	                    DELETE
	                    FROM dbo.FriendRequests
	                    WHERE
		                    FriendRequestId = @FriendRequestId;
                        END
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { requesterUserId, receiverUserId });
        }

        public async Task ReactToPost(PostReactionRequest request, string userId)
        {
            var query = @"
                    DELETE
                    FROM dbo.PostReactions
                    WHERE UserId = @UserID AND PostId = @PostId

                    IF @ReactionType != 'NO-VOTE'
                    INSERT INTO [dbo].[PostReactions]
                        ([UserId]
                        ,[PostId]
                        ,[ReactionType]
                        ,[Timestamp])
                    VALUES
                        (@UserId,
                        @PostId,
                        @ReactionType,
                        @Timestamp)
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { userId, request.ReactionType, request.PostId, Timestamp = DateTime.UtcNow });
        }

        public async Task CommentOnPost(PostCommentRequest request, string userId)
        {
            var query = @"
                    INSERT INTO [dbo].[PostComments]
                        ([UserId]
                        ,[PostId]
                        ,[Comment]
                        ,[Timestamp])
                    VALUES
                        (@UserId,
                        @PostId,
                        @Comment,
                        @Timestamp)
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { request.PostId, request.Comment, userId, Timestamp = DateTime.UtcNow });
        }

        public async Task<IEnumerable<PostComment>> GetCommentsForPost(string postId)
        {
            var query = @"
                    Select
	                    c.CommentId,
	                    c.PostId,
	                    c.Comment,
	                    c.Timestamp,
	                    u.UserName AS 'Username',
	                    a.ProfilePictureUrl,
	                    a.FirstName,
	                    a.LastName,
	                    c.UserId
                    from dbo.PostComments c
                    join dbo.Accounts a on a.User_id = c.UserId
                    join dbo.AspNetUsers u on u.Id = c.UserId
                    where c.PostId = @PostId
                    ;";

            using var connection = GetSqlConnection();
            var comments = await connection.QueryAsync<PostComment>(query, new { postId });
            return comments;
        }
    }
}
