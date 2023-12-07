using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TradeHarborApi.Common;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models.Dtos;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IConfiguration _configuration;
        private readonly AuthRepository _authRepository;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> _optionsMonitor,
            IConfiguration configuration,
            AuthRepository authRepository)
        {
            _userManager = userManager;
            _jwtConfig = _optionsMonitor.CurrentValue;
            _configuration = configuration;
            _authRepository = authRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> GetEmail()
        {
            var allClaims = User?.Claims.Select(c => new { c.Type, c.Value });
            var email = User?.Claims
                .Select(c => new { c.Type, c.Value })
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email);
            return Ok(new { allClaims, email });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);
                if (emailExist != null)
                {
                    return BadRequest(error: Constants.EMAIL_ALREADY_EXISTS);
                }
                else
                {
                    var newUser = new IdentityUser()
                    {
                        Email = requestDto.Email,
                        UserName = requestDto.Username
                    };

                    var usernameExist = await _userManager.FindByNameAsync(requestDto.Username);
                    if (usernameExist == null)
                    {
                        var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
                        if (isCreated.Succeeded)
                        {
                            var linkedRequestDto = new LinkedAccountDto()
                            {
                                FirstName = requestDto.FirstName,
                                LastName = requestDto.LastName,
                                ProfilePictureUrl = requestDto.ProfilePictureUrl,
                                UserId = newUser.Id
                            };
                            await _authRepository.InsertLinkedAccountInformation(linkedRequestDto);

                            var token = await GenerateJwtToken(newUser);
                            return Ok(new RegistrationRequestResponse()
                            {
                                Result = true,
                                Token = token
                            });
                        }
                        else
                        {
                            return BadRequest(error: Constants.REQUEST_FAILED);
                        }
                    }
                    else
                    {
                        return BadRequest(error: Constants.USERNAME_ALREADY_EXISTS);
                    }
                }
            }
            else
            {
                return BadRequest(error: Constants.INCOMPLETE_REQUEST);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);

                if (existingUser != null)
                {
                    var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requestDto.Password);
                    if (isPasswordValid)
                    {
                        var token = await GenerateJwtToken(existingUser);
                        return Ok(new LoginRequestResponse()
                        {
                            Token = token,
                            Result = true
                        });
                    }
                    else
                    {
                        return BadRequest(error: Constants.INVALID_AUTHENTICATION);
                    }
                }
                else
                {
                    return BadRequest(error: Constants.INVALID_AUTHENTICATION);
                }
            }
            else
            {
                return BadRequest(error: Constants.INCOMPLETE_REQUEST);
            }
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
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