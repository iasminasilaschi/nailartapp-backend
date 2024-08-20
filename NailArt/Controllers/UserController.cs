using Microsoft.AspNetCore.Mvc;
using NailArt.Models;
using NailArt.Services;

namespace NailArtApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            //var result = _userService.RegisterUser(model.Username, model.Password, model.Role);
            //if (!result.Success)
            //{
            //    return BadRequest(result.Message);
            //}
            return Ok("result.Message");
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var result = _userService.AuthenticateUser(username, password);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(new { token = result.Token });
        }
    }

}
