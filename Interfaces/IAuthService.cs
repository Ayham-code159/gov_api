using gov_API.DTOs.Auth;

namespace gov_API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}