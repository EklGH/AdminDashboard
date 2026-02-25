using AdminDashboard.Domain.Entities;
using AdminDashboard.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminDashboard.Infrastructure.Auth
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        // Constructeur
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        // ======== Génère un JWT
        public string GenerateToken(User user)
        {
            // claims
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

            // credentials de signature
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT_KEY"]!)
            );

            // Création du JWT
            var token = new JwtSecurityToken(
                issuer: _config["JWT_ISSUER"],
                audience: _config["JWT_AUDIENCE"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
