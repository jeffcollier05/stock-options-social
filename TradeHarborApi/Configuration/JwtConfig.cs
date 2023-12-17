namespace TradeHarborApi.Configuration
{
    /// <summary>
    /// Configuration class for JWT (JSON Web Token) settings.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Gets or sets the secret key used for JWT token generation and validation.
        /// </summary>
        public string Secret { get; set; } = string.Empty;
    }
}
