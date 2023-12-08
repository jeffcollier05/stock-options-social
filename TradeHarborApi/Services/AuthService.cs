using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradeHarborApi.Common;
using TradeHarborApi.Configuration;
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
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(Constants.CLAIM_ID);
            }
            return result;
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
                    new Claim(type: Constants.CLAIM_ID, value: user.Id),
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.UserName),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: "FirstName", value: linkedAccount.FirstName),
                    new Claim(type: "LastName", value: linkedAccount.LastName),
                    new Claim(type: "ProfilePictureUrl", value: linkedAccount.ProfilePictureUrl),
                    new Claim(type: "Username", value: user.UserName),
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
