using Dapper;
using System.Data;
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
            var procedure = "dbo.InsertLinkedAccountInformation";
            using var connection = GetSqlConnection();
            await connection.QueryAsync(procedure, request, commandType: CommandType.StoredProcedure);
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
            var procedure = "dbo.FindLinkedAccountInformation";
            using var connection = GetSqlConnection();
            var linkedAccounts = await connection.QueryAsync<LinkedAccountDto>(procedure, new { userId }, commandType: CommandType.StoredProcedure);
            return linkedAccounts.Single();
        }
    }
}
