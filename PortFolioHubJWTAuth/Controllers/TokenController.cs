using Microsoft.AspNetCore.Mvc;
using DAL;
using PortFolioHubJWTAuth.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PortFolioHubJWTAuth.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly JwtTokenService _jwtTokenService;

        public TokenController(IConfiguration configuration, JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User _userData)
        {
            if (_userData == null || _userData.Username == null || _userData.Password == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _jwtTokenService.GenereateAuthToken(_userData);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            var validatedToken = _jwtTokenService.ValidateToken(token);

            return Ok(validatedToken);
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(User _userData)
        {
            if (_userData == null || _userData.Username == null || _userData.Password == null)
            {
                return BadRequest("Invalid credentials");
            }

            _jwtTokenService._userDAO.AddUser(_userData);

            return Ok(_userData);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUsers(string id)
        {
            User? user = _jwtTokenService._userDAO.GetUser(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            User? user = _jwtTokenService._userDAO.GetUser(id);
            if (user == null) return NotFound();
            _jwtTokenService._userDAO.DeleteUser(id);
            return Ok();
        }
    }
}
