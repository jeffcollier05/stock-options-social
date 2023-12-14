namespace TradeHarborApi.Common
{
    /// <summary>
    /// Defines constants for common request response messages in TradeHarbor.
    /// </summary>
    public static class RequestResponse
    {
        /// <summary>
        /// Message for invalid authentication.
        /// </summary>
        public const string INVALID_AUTHENTICATION = "Invalid authentication.";

        /// <summary>
        /// Message for incomplete requests.
        /// </summary>
        public const string INCOMPLETE_REQUEST = "The request is missing information.";

        /// <summary>
        /// Message for failed request processing.
        /// </summary>
        public const string REQUEST_FAILED = "Error processing the request, please try again later.";

        /// <summary>
        /// Message for an existing email.
        /// </summary>
        public const string EMAIL_ALREADY_EXISTS = "The email already exists.";

        /// <summary>
        /// Message for an existing username.
        /// </summary>
        public const string USERNAME_ALREADY_EXISTS = "The username already exists.";
    }
}
