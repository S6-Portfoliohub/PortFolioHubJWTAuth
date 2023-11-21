using Microsoft.AspNetCore.Mvc;
using PortFolioHubJWTAuth.Models;
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

        [HttpPost]
        public async Task<IActionResult> Post(User _userData)
        {
            if (_userData == null || _userData.Username == null || _userData.Password == null)
            {
                return BadRequest("Invalid credentials");
            }

            var token = _jwtTokenService.GenereateAuthToken(_userData);

            if (token == null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}
