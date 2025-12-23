using Microsoft.AspNetCore.Identity;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;

namespace Mini.CoWorkify.Infrastructure.Services;

public class AuthService(UserManager<IdentityUser> userManager) : IAuthService
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
}