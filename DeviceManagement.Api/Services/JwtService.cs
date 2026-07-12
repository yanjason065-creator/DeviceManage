using DeviceManagement.Api.DTOs.Auth;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace DeviceManagement.Api.Services
{
    public class JwtService:IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public LoginResponseDto GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
                );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                );
            var expires = DateTime.UtcNow.AddMinutes(
                double.Parse(jwtSettings["ExpireMinutes"]!)
                );

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new LoginResponseDto
            {
                Token = tokenString,
                ExpiresAt = expires,
                Username = user.Username,
                Role = user.Role,
            };

            
        }
    }
}
