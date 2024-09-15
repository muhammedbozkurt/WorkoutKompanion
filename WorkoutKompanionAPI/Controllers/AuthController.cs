using Microsoft.AspNetCore.Mvc;
using WorkoutKompanionAPI.Models;
using WorkoutKompanionAPI.Security;

namespace WorkoutKompanionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel.Username == "admin" && loginModel.Password == "password")
            {
                var token = _tokenService.GenerateToken(loginModel.Username);
                return Ok(new { token });
            }
            return Unauthorized("Invalid username or password");
        }
    }
}
