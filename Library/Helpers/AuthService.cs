using Library.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityPulse.Helpers
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JWT>();

            var claims = new List<Claim>
        {
            //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.UserType),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())

        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(jwtSettings.ExpiresInHours),
                signingCredentials: credentials
            );

           
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
