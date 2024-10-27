using Microsoft.IdentityModel.Tokens;
using RazorERPWebAPI.Interface;
using RazorERPWebAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RazorERPWebAPI.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
            
        public Task<string> GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.Role, user.Roles),
            new Claim("CompanyId", user.CompanyId.ToString()),
            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
