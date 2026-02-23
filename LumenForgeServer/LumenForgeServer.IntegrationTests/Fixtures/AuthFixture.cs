using System.IdentityModel.Tokens.Jwt;
using LumenForgeServer.IntegrationTests.Utils;

namespace LumenForgeServer.IntegrationTests.Client;

using System.Net.Http.Headers;

public sealed class AuthFixture : IAsyncLifetime
{
    public JwtSecurityToken AccessToken { get; private set; }
    public string AccessTokenString { get; private set; } = "";
    public HttpClient ApiClient { get; }
    
    public readonly KcOptions KcOptions;

    private readonly HttpClient _kcHttp;
    

    public AuthFixture()
    {
        KcOptions = KcOptions.FromEnvironment();

        _kcHttp = new HttpClient();

        ApiClient = new HttpClient
        {
            BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_BASEURL")
                                  ?? "https://localhost:7217")
        };
    }

    public async Task InitializeAsync()
    {
        var tokenClient = new KeycloakTestClient(_kcHttp);
        AccessTokenString = await tokenClient.GetAccessTokenPasswordGrantAsync(KcOptions.FromEnvironment().KeycloakOptions);
        AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(AccessTokenString);

        ApiClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", AccessTokenString);
    }

    public Task DisposeAsync()
    {
        _kcHttp.Dispose();
        ApiClient.Dispose();
        return Task.CompletedTask;
    }
}
