using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;

namespace Mini.CoWorkify.Infrastructure.Services;

public class AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration) : IAuthService
{
    public async Task<IEnumerable<string>> RegisterAsync(RegisterUserDto dto)
    {
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        return result.Succeeded ? [] : 
            result.Errors.Select(e => e.Description);
    }

    public async Task<string?> LoginAsync(LoginUserDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null) return null;

        var result = await userManager.CheckPasswordAsync(user, dto.Password);
        if (!result) return null;

        // Generar el JWT
        // Los "Claims" son los datos que van incrustados (ID, Email, etc.)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = creds,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}