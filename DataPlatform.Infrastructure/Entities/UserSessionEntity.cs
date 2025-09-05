using System.ComponentModel.DataAnnotations;

namespace DataPlatform.Infrastructure.Entities
{
    public class UserSessionEntity
    {
        public int Id { get; set; }

        [Required]
        public string SessionToken { get; set; } = "";

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public UserEntity User { get; set; } = null!;
    }
}