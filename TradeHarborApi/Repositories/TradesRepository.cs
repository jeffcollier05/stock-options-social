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

        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            //var query = @"
            //        SELECT 
            //            t.[User_id] as UserId,
            //            t.Ticker,
            //            p.Position,
            //            op.[Option],
            //            t.Strikeprice,
            //            t.Comment,
            //            t.[Timestamp],
            //            a.FirstName,
            //            a.LastName,
            //            a.Username,
            //            a.ProfilePictureUrl
            //        FROM dbo.trades t
            //        JOIN reference.[Option] op on op.Option_id = t.[Option]
            //        JOIN reference.[Position] p on p.Position_id = t.Position
            //        JOIN dbo.[Accounts] a on a.User_id = t.User_id
            //        ";

            var query = @"
                    SELECT 
                        t.Ticker,
                        p.Position,
                        op.[Option],
                        t.Strikeprice,
                        t.Comment,
                        t.[Timestamp],
                        a.FirstName,
                        a.LastName,
                        a.ProfilePictureUrl,
                        u.UserName as Username
                    FROM dbo.trades t
                    JOIN reference.[Option] op on op.Option_id = t.[Option]
                    JOIN reference.[Position] p on p.Position_id = t.Position
                    JOIN dbo.Accounts a on a.User_id = t.User_id
                    JOIN dbo.AspNetUsers u on u.Id = t.User_id
                    ";

            using var connection = GetSqlConnection();
            var tradePosts = await connection.QueryAsync<TradePost>(query);
            return tradePosts;
        }

        public async Task<object> CreateTradePost(CreateTradePostRequest request)
        {
            var query = @"
                    DECLARE @PositionId INT;
                    SET @PositionId = (select Position_id from reference.Position where Position = @Position);

                    DECLARE @OptionId INT;
                    SET @OptionId = (select Option_id from reference.[Option] where [Option] = @Option); 

                    INSERT INTO [dbo].[Trades]
                        ([User_id], [Ticker], [Position], [Option], [Strikeprice], [Comment], [Timestamp])
                    VALUES (@Id, @Ticker, @PositionId, @OptionId, @Strikeprice, @Comment, @Timestamp);

                    DECLARE @PrimaryKey INT;
                    SET @PrimaryKey = SCOPE_IDENTITY();

                    SELECT @PrimaryKey AS 'NewPrimaryKey';
                    ";

            using var connection = GetSqlConnection();
            var tradePosts = await connection.QueryAsync<object>(query, request);
            return tradePosts;
        }
    }
}
