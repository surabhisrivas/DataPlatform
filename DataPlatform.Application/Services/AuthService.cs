using DataPlatform.Application.Interfaces;
using DataPlatform.Infrastructure.Entities;
using DataPlatform.Infrastructure.Interfaces;
using DataPlatform.Shared.DTOs;
using System.Security.Cryptography;

namespace DataPlatform.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            // Generate session token
            var sessionToken = GenerateSessionToken();
            var expiresAt = DateTime.UtcNow.AddHours(24); // 24 hours session

            var session = new UserSessionEntity
            {
                SessionToken = sessionToken,
                UserId = user.Id,
                ExpiresAt = expiresAt,
                IsActive = true
            };

            await _userRepository.CreateSessionAsync(session);

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);

            return new LoginResponseDto
            {
                SessionToken = sessionToken,
                ExpiresAt = expiresAt,
                User = MapToUserDto(user)
            };
        }

        public async Task<bool> ValidateSessionAsync(string sessionToken)
        {
            var session = await _userRepository.GetSessionAsync(sessionToken);

            if (session == null || session.ExpiresAt <= DateTime.UtcNow || !session.User.IsActive)
            {
                if (session != null)
                {
                    session.IsActive = false;
                    await _userRepository.UpdateSessionAsync(session);
                }
                return false;
            }

            return true;
        }

        public async Task<UserDto?> GetUserBySessionAsync(string sessionToken)
        {
            var session = await _userRepository.GetSessionAsync(sessionToken);

            if (session == null || session.ExpiresAt <= DateTime.UtcNow || !session.User.IsActive)
                return null;

            return MapToUserDto(session.User);
        }

        public async Task LogoutAsync(string sessionToken)
        {
            var session = await _userRepository.GetSessionAsync(sessionToken);

            if (session != null)
            {
                session.IsActive = false;
                await _userRepository.UpdateSessionAsync(session);
            }
        }

        private static string GenerateSessionToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static UserDto MapToUserDto(UserEntity user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive
            };
        }
    }
}