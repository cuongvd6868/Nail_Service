using Nail_Service.Models;
using Nail_Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nail_Service.DTOs.AccountDto;

namespace Nail_Service.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ISendMailService _sendMailService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, ISendMailService sendMailService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _sendMailService = sendMailService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Guest");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            Username = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(createUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);
                if (user == null)
                    return Unauthorized("Invalid username or password");

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordValid)
                    return Unauthorized("Invalid username or password");

                //await _sendMailService.SendEmailAsync(
                //    user.Email,
                //    "Login Notification",
                //    $"You have logged in at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                // Tạo response DTO
                var userDto = new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An error occurred during login.");

                return StatusCode(500, new { Message = "An internal error occurred. Please try again later." });
            }
        }

    }
}
