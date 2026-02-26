using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using LumenForgeServer.IntegrationTests.Client;

namespace LumenForgeServer.IntegrationTests.Fixtures;

/// <summary>
/// Fixture for provisioning authenticated Keycloak test clients.
/// </summary>
public sealed class AuthFixture : IAsyncLifetime
{
    private KcAdminClient KcAdminApiClient { get; }
    
    public readonly KcOptions _kcOptions;
    

    public AuthFixture()
    {
        _kcOptions = KcOptions.FromEnvironment();

        KcAdminApiClient = new KcAdminClient(_kcOptions);
    }

    public async Task InitializeAsync()
    {
        
    }

    public async Task<KcClient> CreateNewTestUserClientAsync(TestUserInfo testUserInfo, CancellationToken ct)
    {
        var kcUserId = await KcAdminApiClient.CreateKcUserAsync(testUserInfo, ct);
        return await KcClient.GenerateKcClientWithAccessToken(kcUserId!,testUserInfo, _kcOptions);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
