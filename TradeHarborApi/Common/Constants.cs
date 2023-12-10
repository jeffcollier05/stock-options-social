namespace TradeHarborApi.Common
{
    public class Constants
    {
        public const string INVALID_AUTHENTICATION = "Invalid authentication.";

        public const string INCOMPLETE_REQUEST = "The request is missing information.";

        public const string REQUEST_FAILED = "Error processing the request, please try again later.";

        public const string EMAIL_ALREADY_EXISTS = "The email already exists.";

        public const string CLAIM_ID = "UserId";

        public const string USERNAME_ALREADY_EXISTS = "The username already exists.";
    }

    public class CustomClaims
    {
        public const string USERNAME = "Username";

        public const string FIRST_NAME = "FirstName";

        public const string LAST_NAME = "LastName";

        public const string PROFILE_PICTURE_URL = "ProfilePictureUrl";
    }
}
