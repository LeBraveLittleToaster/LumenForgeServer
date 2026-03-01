// ApiHostFixture.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LumenForgeServer.IntegrationTests.Fixtures;

/// <summary>
/// Boots the API host once per test collection. Ensures Program.cs runs (and DevDbSeeder runs in Development).
/// </summary>
public sealed class ApiHostFixture : IAsyncLifetime
{
    public WebApplicationFactory<LumenForgeServer.Program> Factory { get; private set; } = null!;
    public HttpClient AnonymousClient { get; private set; } = null!;

    public Task InitializeAsync()
    {
        Factory = new WebApplicationFactory<LumenForgeServer.Program>()
            .WithWebHostBuilder(b => b.UseEnvironment("Development"));

        // This boots the host -> Program.cs executes -> DevDbSeeder executes (inside IsDevelopment()).
        AnonymousClient = Factory.CreateClient();

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        AnonymousClient.Dispose();
        await Factory.DisposeAsync();
    }
}