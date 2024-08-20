using Microsoft.AspNetCore.Mvc;
using NailArt.Models;
using NailArt.Services;
using NailArtApp.Models;

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
        public IActionResult Register([FromBody] RegisterUserModel model)
        {
            var (Success, Message) = _userService.RegisterUser(model.Username, model.Password, model.Role);
            if (!Success)
            {
                return BadRequest(Message);
            }

            return Ok(Message);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserModel model)
        {
            var (Success, Token, Message) = _userService.AuthenticateUser(model.Username, model.Password);
            if (!Success)
            {
                return BadRequest(Message);
            }
            return Ok(Token);
        }
    }

}
