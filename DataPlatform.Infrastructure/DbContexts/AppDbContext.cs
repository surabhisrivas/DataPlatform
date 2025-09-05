using DataPlatform.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataPlatform.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserSessionEntity> UserSessions { get; set; }
        public DbSet<LocalizationEntity> Localizations { get; set; }
        public DbSet<ApiRequestLogEntity> ApiRequestLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Session relationship
            modelBuilder.Entity<UserSessionEntity>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-ApiRequestLog relationship
            modelBuilder.Entity<ApiRequestLogEntity>()
                .HasOne(log => log.User)
                .WithMany()
                .HasForeignKey(log => log.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Localization unique constraint
            modelBuilder.Entity<LocalizationEntity>()
                .HasIndex(l => new { l.Key, l.Language })
                .IsUnique();

            // User unique constraints
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed default data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default users
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@dataplatform.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123", BCrypt.Net.BCrypt.GenerateSalt()),
                    Salt = BCrypt.Net.BCrypt.GenerateSalt(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new UserEntity
                {
                    Id = 2,
                    Username = "user1",
                    Email = "user1@dataplatform.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123", BCrypt.Net.BCrypt.GenerateSalt()),
                    Salt = BCrypt.Net.BCrypt.GenerateSalt(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed default localization data
            var localizationData = new[]
            {
                // English
                new LocalizationEntity { Id = 1, Key = "employee.name", Language = "en", Value = "Name" },
                new LocalizationEntity { Id = 2, Key = "employee.email", Language = "en", Value = "Email" },
                new LocalizationEntity { Id = 3, Key = "employee.phone", Language = "en", Value = "Phone" },
                new LocalizationEntity { Id = 4, Key = "employee.department", Language = "en", Value = "Department" },
                new LocalizationEntity { Id = 5, Key = "employee.position", Language = "en", Value = "Position" },
                new LocalizationEntity { Id = 6, Key = "salary.amount", Language = "en", Value = "Salary Amount" },
                new LocalizationEntity { Id = 7, Key = "salary.currency", Language = "en", Value = "Currency" },

                // Spanish
                new LocalizationEntity { Id = 8, Key = "employee.name", Language = "es", Value = "Nombre" },
                new LocalizationEntity { Id = 9, Key = "employee.email", Language = "es", Value = "Correo electrónico" },
                new LocalizationEntity { Id = 10, Key = "employee.phone", Language = "es", Value = "Teléfono" },
                new LocalizationEntity { Id = 11, Key = "employee.department", Language = "es", Value = "Departamento" },
                new LocalizationEntity { Id = 12, Key = "employee.position", Language = "es", Value = "Posición" },
                new LocalizationEntity { Id = 13, Key = "salary.amount", Language = "es", Value = "Monto del salario" },
                new LocalizationEntity { Id = 14, Key = "salary.currency", Language = "es", Value = "Moneda" }
            };

            modelBuilder.Entity<LocalizationEntity>().HasData(localizationData);
        }
    }

    public class SqlServerDbContext : AppDbContext
    {
        public SqlServerDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }

    public class PostgresDbContext : AppDbContext
    {
        public PostgresDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}
