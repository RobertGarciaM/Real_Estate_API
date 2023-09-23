using DTOModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;


namespace RealEstate.Mediator.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(UserCredentialsDto credentials)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, credentials.Password))
            {
                return string.Empty;
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            string token = GenerateJwtToken(user, userRoles);

            return token;
        }

        private string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsIdentity claims = new(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            });

            foreach (string userRole in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, userRole));
            }

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Tokens:Key"])), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Security:Tokens:Issuer"],
                Audience = _configuration["Security:Tokens:Audience"]
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
