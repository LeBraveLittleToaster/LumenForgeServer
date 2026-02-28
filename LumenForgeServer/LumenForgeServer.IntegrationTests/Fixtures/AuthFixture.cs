using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LumenForgeServer.Auth.Client;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Fixtures;

/// <summary>
/// Fixture for provisioning authenticated Keycloak test clients.
/// </summary>
public sealed class AuthFixture : IAsyncLifetime
{
    public static string UsernamePrefix = "test";
    private readonly KcClientOptions _kcOptions = KcClientOptions.FromEnvironment();
    public KcClient AdminClient;

    
    public async Task InitializeAsync()
    {
        AdminClient = KcClient.GenerateKcClientWithAccessTokenAsync(_kcOptions, CancellationToken.None).Result;
    }

    public async Task CreateNewUserWithRoles(TestUserInfo testUserInfo, HashSet<Role> roles)
    {
        var user = await AdminClient.AdminClient.PutAsJsonAsync("/api/v1/auth/users",testUserInfo.ToAddKcUserDto());
        Console.WriteLine(user);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
