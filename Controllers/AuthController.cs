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
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(UserDto dto)
        //{
        //    if (StringHelper.NullOrEmptyChecker(dto.Username, dto.Password))
        //        return BadRequest("Username or Password can't be empty");

        //    var registered = await authService.RegisterAsync(dto);
        //}

    }
}
