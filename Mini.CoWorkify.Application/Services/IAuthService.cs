using Mini.CoWorkify.Application.DTOs;

namespace Mini.CoWorkify.Application.Services;

public interface IAuthService
{
    Task<IEnumerable<string>> RegisterAsync(RegisterUserDto dto);
    Task<string?> LoginAsync(LoginUserDto dto);
}