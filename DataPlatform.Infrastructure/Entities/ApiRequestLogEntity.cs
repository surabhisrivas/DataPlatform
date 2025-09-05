using System.ComponentModel.DataAnnotations;

namespace DataPlatform.Infrastructure.Entities
{
    public class ApiRequestLogEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Endpoint { get; set; } = "";

        public DateTime RequestTime { get; set; } = DateTime.UtcNow;

        [MaxLength(45)]
        public string IpAddress { get; set; } = "";

        // Navigation properties
        public UserEntity User { get; set; } = null!;
    }
}