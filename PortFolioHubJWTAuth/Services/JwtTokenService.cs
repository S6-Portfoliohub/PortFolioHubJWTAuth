using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using PortFolioHubJWTAuth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortFolioHubJWTAuth.Services
{
    public class JwtTokenService
    {

        public IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        private readonly List<User> _users = new()
        {
            new("test@test.com", "test", " "),
            new("test@test.com", "test", "testUid2")
        };

        public JwtSecurityToken? GenereateAuthToken(User userCredentials)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == userCredentials.Username &&
                u.Password == userCredentials.Password
            );

            if (user is null)
            {
                return null;
            }

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

            return token;
        }
    }
}
