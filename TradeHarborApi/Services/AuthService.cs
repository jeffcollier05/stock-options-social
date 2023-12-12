using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly AuthRepository _authRepository;
        private readonly JwtConfig _jwtConfig;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            AuthRepository authRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _authRepository = authRepository;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public string GetUserIdFromJwt()
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(TradeHarborClaims.CLAIM_ID) ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("UserId from the JWT is null.");
            }

            return userId;
        }

        public JwtUserProfileDto GetUserProfileFromJwt()
        {
            var profile = new JwtUserProfileDto();
            if (_httpContextAccessor.HttpContext != null)
            {
                profile = new JwtUserProfileDto
                {
                    UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(TradeHarborClaims.CLAIM_ID),
                    Username = _httpContextAccessor.HttpContext.User.FindFirstValue(TradeHarborClaims.USERNAME),
                    FirstName = _httpContextAccessor.HttpContext.User.FindFirstValue(TradeHarborClaims.FIRST_NAME),
                    LastName = _httpContextAccessor.HttpContext.User.FindFirstValue(TradeHarborClaims.LAST_NAME),
                    ProfilePictureUrl = _httpContextAccessor.HttpContext.User.FindFirstValue(TradeHarborClaims.PROFILE_PICTURE_URL)
                };
            }
            return profile;
        }

        public async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var linkedAccount = await _authRepository.FindLinkedAccountInformation(user.Id);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type: JwtRegisteredClaimNames.Iss, value: _configuration.GetSection(key: "JwtConfig:ValidIssuer")?.Value),
                    new Claim(type: JwtRegisteredClaimNames.Aud, value: _configuration.GetSection(key: "JwtConfig:ValidAudience")?.Value),
                    new Claim(type: TradeHarborClaims.CLAIM_ID, value: user.Id),
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.UserName),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: TradeHarborClaims.FIRST_NAME, value: linkedAccount.FirstName),
                    new Claim(type: TradeHarborClaims.LAST_NAME, value: linkedAccount.LastName),
                    new Claim(type: TradeHarborClaims.PROFILE_PICTURE_URL, value: linkedAccount.ProfilePictureUrl),
                    new Claim(type: TradeHarborClaims.USERNAME, value: user.UserName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    algorithm: SecurityAlgorithms.HmacSha256)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
