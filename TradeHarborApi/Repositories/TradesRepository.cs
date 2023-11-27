using System.Data.Common;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using TradeHarborApi.Configuration;

namespace TradeHarborApi.Repositories
{
    public class TradesRepository
    {
        private readonly IApiConfiguration _config;

        public TradesRepository(IApiConfiguration config)
        {
            _config = config;
        }

        private DbConnection GetConnection()
        {
            var connection = new SqlConnection(_config.SqlConnectionString);
            connection.Open();
            return connection;
        }

        public async Task<IEnumerable<object>> GetTrades()
        {
            var query = "select * from dbo.Trades";
            using var connection = GetConnection();
            var asdf = await connection.QueryAsync<object>(query);
            return asdf;
        }
    }
}
