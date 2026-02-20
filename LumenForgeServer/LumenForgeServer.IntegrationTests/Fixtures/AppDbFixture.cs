using LumenForgeServer.Common.Database;

namespace LumenForgeServer.IntegrationTests.Fixtures;

using Microsoft.EntityFrameworkCore;

public class AppDbFixture
{
    public string ConnectionString { get; }

    public AppDbFixture()
    {
        ConnectionString = "Host=localhost;Port=5432;Database=lumenforge;Username=postgres;Password=mypassword";
    }

    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString, o => o.UseNodaTime())
            .Options;

        return new AppDbContext(options);
    }
}