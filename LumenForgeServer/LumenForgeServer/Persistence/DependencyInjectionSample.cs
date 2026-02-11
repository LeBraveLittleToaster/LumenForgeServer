using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RentalDomain.Persistence;

public static class DependencyInjectionSample
{
    public static IServiceCollection AddAppDb(this IServiceCollection services, string connectionString)
    {
        // Example: SQL Server
        // services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));

        // Example: PostgreSQL (Npgsql)
        // services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));

        // Placeholder: choose your provider
        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));
        return services;
    }
}
