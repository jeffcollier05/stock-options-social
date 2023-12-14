using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Data.SqlClient;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models.AuthDtos;

namespace TradeHarborApi.Repositories
{
    /// <summary>
    /// Repository for handling authentication-related data operations.
    /// </summary>
    public class AuthRepository
    {
        private readonly IApiConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRepository"/> class.
        /// </summary>
        public AuthRepository(IApiConfiguration config)
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
        /// Inserts linked account information into the database.
        /// </summary>
        /// <param name="request">The linked account information to be inserted.</param>
        /// <returns>An asynchronous task representing the insert operation.</returns>
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

        /// <summary>
        /// Finds linked account information based on the provided user ID.
        /// </summary>
        /// <param name="userId">The user ID for which linked account information is retrieved.</param>
        /// <returns>
        /// An asynchronous task representing the retrieval operation.
        /// The task result is a <see cref="LinkedAccountDto"/> containing linked account information.
        /// </returns>
        public async Task<LinkedAccountDto> FindLinkedAccountInformation(string userId)
        {
            var query = @"
                    SELECT FirstName, LastName, ProfilePictureUrl
                    FROM [TradeHarbor].[dbo].[Accounts]
                    WHERE [User_id] = @UserId
                    ";

            using var connection = GetSqlConnection();
            var linkedAccounts = await connection.QueryAsync<LinkedAccountDto>(query, new { userId });
            return linkedAccounts.Single();
        }
    }
}
