using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Data.SqlClient;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models.AuthDtos;

namespace TradeHarborApi.Repositories
{
    public class AuthRepository
    {
        private readonly IApiConfiguration _config;

        public AuthRepository(IApiConfiguration config)
        {
            _config = config;
        }

        private DbConnection GetSqlConnection()
        {
            var connection = new SqlConnection(_config.SqlConnectionString);
            connection.Open();
            return connection;
        }

        public async Task InsertLinkedAccountInformation(LinkedAccountDto request)
        {
            var query = @"
                    INSERT INTO [dbo].[Accounts]
                        ([User_id]
                        ,[FirstName]
                        ,[LastName]
                        ,[ProfilePictureUrl])
                    VALUES
                        (@UserId,
                        @FirstName,
                        @LastName,
                        @ProfilePictureUrl)
                    ";

            using var connection = GetSqlConnection();
            await connection.QueryAsync(query, request);
        }

        public async Task<LinkedAccountDto> FindLinkedAccountInformation(string userId)
        {
            var query = @"
                    SELECT FirstName, LastName, ProfilePictureUrl
                    FROM [TradeHarbor].[dbo].[Accounts]
                    WHERE [User_id] = @UserId
                    ";

            using var connection = GetSqlConnection();
            var linkedAccount = await connection.QueryAsync<LinkedAccountDto>(query, new { userId });

            if (!linkedAccount.IsNullOrEmpty() && linkedAccount.Count() == 1)
            {
                return linkedAccount.FirstOrDefault();
            }
            else
            {
                throw new InvalidOperationException("Linked account not found or too many.");
            }

        }
    }
}
