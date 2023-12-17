using System.Collections.Generic;

namespace TradeHarborApi.Models.AuthDtos
{
    /// <summary>
    /// Represents the result of an authentication operation.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the authentication operation was successful.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Gets or sets a list of error messages, if any, encountered during the authentication operation.
        /// </summary>
        public List<string> Errors { get; set; } = [];
    }
}
