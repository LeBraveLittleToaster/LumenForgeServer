using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml;
using LumenForgeServer.Auth.Client;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto.Command;
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
    }

    public async Task CreateNewUser()
    {
        var apiHttpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7217")
        };
        var guid = Guid.NewGuid();
        var dto = new AddKcUserDto
        {
            Username = $"test{guid}",
            Password = $"pwtoaster{guid}",
            Email = $"toaster{guid}@toaster.de",
            FirstName = $"Pascal",
            LastName = $"Toaster",
            Groups = ["admins", "users"]
        };
        var response = await apiHttpClient.PutAsJsonAsync("/api/v1/auth/users", dto);

        var kcHttpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:8080")
        };
        var data = new Dictionary<string, string>
        {
            ["username"] = dto.Username,
            ["password"] = dto.Password,
            ["grant_type"] = "password",
            ["client_id"] = "lumenforge-test",
        };

        using var content = new FormUrlEncodedContent(data);

        try
        {
            var url = $"/realms/lumenforge-realm/protocol/openid-connect/token";
            var resp = await kcHttpClient.PostAsync(url, content, CancellationToken.None);

            var body = await resp.Content.ReadAsStringAsync(CancellationToken.None);
            var respJson = JsonSerializer.Deserialize<JsonElement>(body);
            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Keycloak token request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");


            var adminToken = respJson.GetProperty("access_token").GetString();

            var AccessTokenString = respJson.GetProperty("access_token").GetString()!;
            var AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(AccessTokenString);

            if (adminToken != null)
            {
                apiHttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessTokenString);
                var roles = await apiHttpClient.GetAsync("api/v1/auth/roles", CancellationToken.None);
                Console.WriteLine(roles);
            }
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}