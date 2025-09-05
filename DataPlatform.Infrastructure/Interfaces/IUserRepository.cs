using DataPlatform.Infrastructure.Entities;

namespace DataPlatform.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByUsernameAsync(string username);
        Task<UserEntity?> GetByIdAsync(int id);
        Task<UserSessionEntity> CreateSessionAsync(UserSessionEntity session);
        Task<UserSessionEntity?> GetSessionAsync(string sessionToken);
        Task UpdateUserAsync(UserEntity user);
        Task UpdateSessionAsync(UserSessionEntity session);
        Task LogApiRequestAsync(ApiRequestLogEntity apiLog);
    }
}