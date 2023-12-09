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
                        u.UserName as Username
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

        public async Task<IEnumerable<FriendProfile>> GetAllUsers()
        {
            var query = @"
                    SELECT u.Id as 'UserId', u.UserName, ac.FirstName, ac.LastName, ac.ProfilePictureUrl
                    FROM dbo.AspNetUsers u
                    JOIN dbo.Accounts ac on ac.User_id = u.Id
                    ;";

            using var connection = GetSqlConnection();
            var users = await connection.QueryAsync<FriendProfile>(query);
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

        public async Task AddFriend(string user1Id, string user2Id)
        {
            var query = @"
                    INSERT INTO [dbo].[FriendPairs]
                        ([Person1Id]
                        ,[Person2Id]
                        ,[PairingDate])
                     VALUES
                        (@User1Id,
                        @User2Id,
                        GETDATE())
                    ;";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, new { user1Id, user2Id });
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
    }
}
