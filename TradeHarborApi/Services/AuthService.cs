using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradeHarborApi.Common;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models.AuthDtos;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    /// <summary>
    /// Service for handling authentication-related operations.
    /// </summary>
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly AuthRepository _authRepository;
        private readonly JwtConfig _jwtConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
        /// <param name="configuration">The configuration interface providing access to application settings.</param>
        /// <param name="optionsMonitor">Monitor for JWT configuration options.</param>
        /// <param name="authRepository">Repository for authentication-related data operations.</param>
        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            AuthRepository authRepository
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _authRepository = authRepository;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        /// <summary>
        /// Retrieves the user ID from the current JWT token in the HTTP context.
        /// </summary>
        /// <returns>The user ID extracted from the JWT token.</returns>
        public string GetUserIdFromJwt()
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(TradeHarborClaims.CLAIM_ID) ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("UserId from the JWT is null.");
            }

            return userId;
        }

        /// <summary>
        /// Retrieves the user profile from the current JWT token in the HTTP context.
        /// </summary>
        /// <returns>The user profile information extracted from the JWT token.</returns>
        public JwtUserProfileDto GetUserProfileFromJwt()
        {
            var profile = new JwtUserProfileDto();
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var claims = httpContext.User.Claims;

                profile = new JwtUserProfileDto
                {
                    UserId = FindClaimValue(claims, TradeHarborClaims.CLAIM_ID),
                    Username = FindClaimValue(claims, TradeHarborClaims.USERNAME),
                    FirstName = FindClaimValue(claims, TradeHarborClaims.FIRST_NAME),
                    LastName = FindClaimValue(claims, TradeHarborClaims.LAST_NAME),
                    ProfilePictureUrl = FindClaimValue(claims, TradeHarborClaims.PROFILE_PICTURE_URL)
                };
            }
            return profile;
        }

        /// <summary>
        /// Finds the value of a specific claim type within a collection of claims.
        /// </summary>
        /// <param name="claims">The collection of claims to search through.</param>
        /// <param name="claimType">The type of the claim to find.</param>
        /// <returns>The value of the specified claim type, or an empty string if not found.</returns>
        private static string FindClaimValue(IEnumerable<Claim> claims, string claimType)
        {
            var claimValue = claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            return claimValue ?? "";
        }

        /// <summary>
        /// Finds the value of a specific configuration key within the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration interface providing access to application settings.</param>
        /// <param name="claimKey">The key for the configuration value to find.</param>
        /// <returns>The value of the specified configuration key.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the configuration value for the specified key is missing.
        /// </exception>
        private static string FindConfigurationValue(IConfiguration configuration, string claimKey)
        {
            var claimValue = configuration.GetSection(key: claimKey)?.Value;
            if (claimValue == null)
            {
                throw new InvalidOperationException($"Missing the configuration value for the key {claimKey}.");
            }

            return claimValue;
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the JWT token is generated.</param>
        /// <returns>The generated JWT token.</returns>
        public async Task<string> GenerateJwtToken(IdentityUser user)
        {
            // Get the secret key
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // Find linked account information
            var linkedAccount = await _authRepository.FindLinkedAccountInformation(user.Id);

            // Configure the JWT token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type: JwtRegisteredClaimNames.Iss, value: FindConfigurationValue(_configuration, "JwtConfig:ValidIssuer")),
                    new Claim(type: JwtRegisteredClaimNames.Aud, value: FindConfigurationValue(_configuration, "JwtConfig:ValidAudience")),
                    new Claim(type: TradeHarborClaims.CLAIM_ID, value: user.Id),
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.UserName ?? ""),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email ?? ""),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: TradeHarborClaims.FIRST_NAME, value: linkedAccount.FirstName),
                    new Claim(type: TradeHarborClaims.LAST_NAME, value: linkedAccount.LastName),
                    new Claim(type: TradeHarborClaims.PROFILE_PICTURE_URL, value: linkedAccount.ProfilePictureUrl),
                    new Claim(type: TradeHarborClaims.USERNAME, value: user.UserName ?? ""),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    algorithm: SecurityAlgorithms.HmacSha256)
            };

            // Create and write the JWT token
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
