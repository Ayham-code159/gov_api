using gov_API.Entities.Dtos.Auth;

namespace gov_API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}