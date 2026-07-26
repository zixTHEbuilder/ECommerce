using ECommerce.Dtos;
using ECommerce.Helpers;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.Controllers
{
    [ApiController]
    [Route("{Controller}")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            if (StringHelper.NullOrEmptyChecker(dto.Username, dto.Password))
                return BadRequest("Username or Password can't be empty");

            var registered = await authService.RegisterAsync(dto);

            return registered? Conflict("User not created - User already exists, Please login")
                             : Ok("User Created, Please login to get User ID, Access Token and Refresh Token");
        }
        [HttpPatch("Login")]
        public async Task<IActionResult> Login(UserDto dto)
        {
            if (StringHelper.NullOrEmptyChecker(dto.Username, dto.Password))
                return BadRequest("Username or Password can't be empty");

            var token = await authService.LoginAsync(dto);

            return token == null? Unauthorized("Incorrect Username or Password") 
                                : Ok(token);
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            if (StringHelper.NullOrEmptyChecker(dto.RefreshToken))
                return BadRequest("Refresh Token can't be empty");

            var token = await authService.RefreshTokenAsync(dto);

            return token == null ? Unauthorized("Invalid Username or Refresh Token")
                                 : Ok(token);
        }
    }
}
