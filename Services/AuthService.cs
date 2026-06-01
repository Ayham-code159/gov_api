using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gov_API.DTOs;
using gov_API.Entities.Dtos.Auth;
using gov_API.Entities.Models;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace gov_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.GovernmentEntity)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("This account is not active.");

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("EntityAdmin") || roles.Contains("EntityUser"))
            {
                if (user.GovernmentEntity == null)
                    throw new UnauthorizedAccessException("User is not linked to a government entity.");

                if (user.GovernmentEntity.Status != EntityStatus.Approved)
                    throw new UnauthorizedAccessException("Government entity is not approved.");
            }

            var token = GenerateJwtToken(user, roles.ToList());

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList(),
                GovernmentEntityId = user.GovernmentEntityId
            };
        }

        private string GenerateJwtToken(ApplicationUser user, List<string> roles)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT key is missing.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("FullName", user.FullName),
                new Claim("GovernmentEntityId", user.GovernmentEntityId?.ToString() ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}