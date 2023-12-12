using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradeHarborApi.Common;
using TradeHarborApi.Models.Dtos;
using TradeHarborApi.Repositories;
using TradeHarborApi.Services;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthRepository _authRepository;
        private readonly AuthService _authService;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            AuthRepository authRepository,
            AuthService authService)
        {
            _userManager = userManager;
            _authRepository = authRepository;
            _authService = authService;
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

                            var token = await _authService.GenerateJwtToken(newUser);
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
                        var token = await _authService.GenerateJwtToken(existingUser);
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
    }
}