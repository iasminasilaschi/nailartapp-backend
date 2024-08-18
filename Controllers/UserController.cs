using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult Register(string username, string password, string role)
    {
        var result = _userService.RegisterUser(username, password, role);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
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

    [Authorize] // <-- Only authenticated users can access this action
    [HttpGet("secure-endpoint")]
    public IActionResult SecureEndpoint()
    {
        return Ok("This is a secure endpoint, only accessible with a valid token.");
    }
}
