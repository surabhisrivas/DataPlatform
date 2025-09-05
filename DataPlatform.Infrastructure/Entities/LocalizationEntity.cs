using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPlatform.Infrastructure.Entities
{
    public class LocalizationEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Key { get; set; } = "";

        [Required]
        [MaxLength(10)]
        public string Language { get; set; } = "";

        [Required]
        public string Value { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}