using financeTrackerBackned.Domain;
using financeTrackerBackned.Dtos;
using financeTrackerBackned.Errors;
using financeTrackerBackned.Services;
using Microsoft.AspNetCore.Mvc;

namespace financeTrackerBackned.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JWTService _jwtService;
        public UserController(UserService userService, JWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto _user)
        {
            if (_user.Email == null || _user.Email == "") return BadRequest("Email is required!!");
            try
            {
                var user = await _userService.Login(_user);
                if (user is User)
                    return Ok(new { token = _jwtService.GenerateJwtToken((User)user) });
                return Unauthorized(new { message = "Email and Password doesn't match!!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto _user)
        {
            if (_user.Email == null || _user.Email == "" ||
                _user.Password == null || _user.Password == "") return BadRequest("Email is required!!");
            try
            {
                var resut = await _userService.Register(_user);
                if (resut is User) return Ok(new { token = _jwtService.GenerateJwtToken((User)resut) });
                else if (resut is UserAlreadyExistError)
                    return BadRequest(new { error = "User already exist" });
                return BadRequest(new { error = "An unknown error occured!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
