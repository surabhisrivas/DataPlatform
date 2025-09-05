using DataPlatform.Infrastructure.Interfaces;
using DataPlatform.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataPlatform.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        }

        public async Task<UserEntity?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<UserSessionEntity> CreateSessionAsync(UserSessionEntity session)
        {
            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<UserSessionEntity?> GetSessionAsync(string sessionToken)
        {
            return await _context.UserSessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.IsActive);
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSessionAsync(UserSessionEntity session)
        {
            _context.UserSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task LogApiRequestAsync(ApiRequestLogEntity apiLog)
        {
            _context.ApiRequestLogs.Add(apiLog);
            await _context.SaveChangesAsync();
        }
    }
}