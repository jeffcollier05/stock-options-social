using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradeHarborApi.Common;
using TradeHarborApi.Configuration;
using TradeHarborApi.Models.Dtos;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> _optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = _optionsMonitor.CurrentValue;
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
                        UserName = requestDto.Email
                    };

                    var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
                    if (isCreated.Succeeded)
                    {
                        var token = GenerateJwtToken(newUser);

                        return Ok(new RegistrationRequestResponse()
                        {
                            Result = true,
                            Token = token
                        });
                    }
                    else
                    {
                        return BadRequest(error: Constants.REQUEST_FAILED);
                        //return BadRequest(error: isCreated.Errors.Select(x => x.Description).ToList());
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
                        var token = GenerateJwtToken(existingUser);
                        return Ok(new RegistrationRequestResponse()
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

        private string GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type: "Id", value: user.Id),
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
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