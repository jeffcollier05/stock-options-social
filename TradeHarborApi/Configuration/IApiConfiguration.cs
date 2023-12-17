namespace TradeHarborApi.Configuration
{
    /// <summary>
    /// Interface for application configuration settings related to the API.
    /// </summary>
    public interface IApiConfiguration
    {
        /// <summary>
        /// Gets the connection string for SQL Server.
        /// </summary>
        string SqlConnectionString { get; }
    }
}
