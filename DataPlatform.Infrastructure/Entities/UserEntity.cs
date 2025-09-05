using System.ComponentModel.DataAnnotations;

namespace DataPlatform.Infrastructure.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = "";

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string Salt { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<UserSessionEntity> Sessions { get; set; } = new List<UserSessionEntity>();
    }
}