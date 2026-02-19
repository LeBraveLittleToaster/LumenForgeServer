using System.IdentityModel.Tokens.Jwt;
using LumenForgeServer.IntegrationTests.Utils;

namespace LumenForgeServer.IntegrationTests.Client;

using System.Net.Http.Headers;

public sealed class AuthFixture : IAsyncLifetime
{
    public JwtSecurityToken AccessToken { get; private set; }
    public string AccessTokenString { get; private set; } = "";
    public HttpClient ApiClient { get; }

    private readonly HttpClient _kcHttp;
    private readonly KeycloakOptions _options;

    public AuthFixture()
    {
        // Configure
        _options = new KeycloakOptions
        {
            BaseUrl  = Environment.GetEnvironmentVariable("KC_BASEURL")  ?? "http://localhost:8080",
            Realm    = Environment.GetEnvironmentVariable("KC_REALM")    ?? "lumenforge-realm",
            ClientId = Environment.GetEnvironmentVariable("KC_CLIENTID") ?? "lumenforge-test",
            Username = Environment.GetEnvironmentVariable("KC_USER")     ?? "alice",
            Password = Environment.GetEnvironmentVariable("KC_PASS")     ?? "alice123",
        };

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
        AccessTokenString = await tokenClient.GetAccessTokenPasswordGrantAsync(_options);
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
