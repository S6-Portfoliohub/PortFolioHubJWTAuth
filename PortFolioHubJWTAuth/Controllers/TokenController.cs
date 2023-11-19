using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortFolioHubJWTAuth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortFolioHubJWTAuth.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        [HttpPost]
        public async Task<IActionResult> Post(User _userData)
        {
            if (_userData != null && _userData.Username != null && _userData.Password != null)
            {
                User user = await GetUserAsync(_userData.Username, _userData.Password);

                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Username),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }

        private async Task<User> GetUserAsync(string username, string password)
        {
            string[] scopes = { "test", "test" };
            return new User(password, username, "", scopes);
        }
    }
}
