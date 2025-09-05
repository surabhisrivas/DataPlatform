using DataPlatform.Infrastructure.Interfaces;
using DataPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataPlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var provider = config["DatabaseProvider"];
            var sqlServerConn = config["ConnectionStrings:SqlServer"];
            var postgresConn = config["ConnectionStrings:Postgres"];

            services.AddDbContext<AppDbContext>(options =>
            {
                if (provider?.ToLower() == "sqlserver")
                    options.UseSqlServer(sqlServerConn);
                else if (provider?.ToLower() == "postgres")
                    options.UseNpgsql(postgresConn);
                else
                    throw new Exception("Database Provider is not configured properly in appsettings.json file.");
            });

            // Register repositories (Infrastructure implementations)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocalizationRepository, LocalizationRepository>();

            return services;
        }
    }
}