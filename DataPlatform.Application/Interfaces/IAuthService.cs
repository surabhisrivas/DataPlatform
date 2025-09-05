using DataPlatform.Shared.DTOs;

namespace DataPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> ValidateSessionAsync(string sessionToken);
        Task<UserDto?> GetUserBySessionAsync(string sessionToken);
        Task LogoutAsync(string sessionToken);
    }
}