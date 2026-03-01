using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Azure;
using FluentAssertions;
using LumenForgeServer.Auth.Client;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.Common.Database;
using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Fixtures;

/// <summary>
/// Fixture for provisioning authenticated Keycloak test clients.
/// </summary>
public sealed class AuthFixture : IAsyncLifetime
{
    public static string UsernamePrefix = "test";
    private readonly KcAndAppClientOptions _kcAndAppOptions = KcAndAppClientOptions.FromEnvironment();
    private KcClient KcAdminUser;
    private TestUserBundle AppAdminUser;


    public async Task InitializeAsync()
    {
        KcAdminUser = await KcClient.GenerateKcClientWithAccessTokenAsync(_kcAndAppOptions, CancellationToken.None);
        AppAdminUser = await GetInitialAdminUserAsync(KcAndAppClientOptions.FromEnvironment());
    }

    public HttpClient GetAnonymousClient()
    {
        return new HttpClient()
        {
            BaseAddress = new Uri(_kcAndAppOptions.AppBaseUrl)
        };
    }

    public async Task<TestUserBundle> GetInitialAdminUserAsync(KcAndAppClientOptions options)
    {
        
        var apiHttpClient = new HttpClient()
        {
            BaseAddress = new Uri(options.AppBaseUrl)
        };

        var (accessTokenString, accessToken) = await GetKcTokenRequestAsync(options,DbInitConstants.InitUsername, DbInitConstants.InitPassword);
        
        apiHttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessTokenString);
        
        return new TestUserBundle(accessToken, apiHttpClient);
    }

    public async Task<TestUserBundle> CreateNewUserAsync(KcAndAppClientOptions options, CreateTestUserDto dto)
    {
        var apiHttpClient = new HttpClient()
        {
            BaseAddress = new Uri(options.AppBaseUrl)
        };
        var response = await apiHttpClient.PutAsJsonAsync("/api/v1/auth/users", dto);
        response.EnsureSuccessStatusCode();

        var (accessTokenString, accessToken) = await GetKcTokenRequestAsync(options,dto.Username, dto.Password);
        
        apiHttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessTokenString);
        
        return new TestUserBundle(accessToken, apiHttpClient);
    }
    
    public async Task<TestUserBundle> CreateNewUserWithRolesAsync(KcAndAppClientOptions options, CreateTestUserDto dto, Role[] roles)
    {
        var apiHttpClient = new HttpClient()
        {
            BaseAddress = new Uri(options.AppBaseUrl)
        };
        var response = await apiHttpClient.PutAsJsonAsync("/api/v1/auth/users", dto);
        if(response.StatusCode != HttpStatusCode.Created) throw new RequestFailedException(response.StatusCode.ToString());

        var (accessTokenString, accessToken) = await GetKcTokenRequestAsync(options,dto.Username, dto.Password);
        
        apiHttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessTokenString);
        
        var guid = Guid.NewGuid().ToString("N");
        var groupView = await CreateGroupWithRolesAsync(AppAdminUser.AppClient, roles, $"TestGroup{guid}", "Test description" + guid);
        var assignedResp = await AssignUserToGroupAsync(AppAdminUser.AppClient, groupView.Guid, accessToken.Subject);
        Console.WriteLine(assignedResp);
        return new TestUserBundle(accessToken, apiHttpClient);
    }

    private async Task<(string? accessTokenString, JwtSecurityToken accessToken)> GetKcTokenRequestAsync(
        KcAndAppClientOptions options, string username, string password)
    {
        var kcHttpClient = new HttpClient()
        {
            BaseAddress = new Uri(options.KcBaseUrl)
        };
        var data = new Dictionary<string, string>
        {
            ["username"] = username,
            ["password"] = password,
            ["grant_type"] = "password",
            ["client_id"] = options.KcTestClientId,
        };
        using var content = new FormUrlEncodedContent(data);
        var url = $"/realms/{options.KcRealm}/protocol/openid-connect/token";
        var resp = await kcHttpClient.PostAsync(url, content, CancellationToken.None);

        var body = await resp.Content.ReadAsStringAsync(CancellationToken.None);
        var respJson = JsonSerializer.Deserialize<JsonElement>(body);
        if (!resp.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Keycloak token request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");

        var accessTokenString = respJson.GetProperty("access_token").GetString()!;
        var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenString);

        return accessTokenString == null
            ? throw new KeycloakException("Failed to get access token")
            : (accessTokenString, accessToken);
    }

    public async Task<GroupView> CreateGroupAsync(HttpClient adminClient, string? name = null, string? description = null)
    {
        var groupName = name ?? "Test Group " + Guid.NewGuid();
        var groupDesc = description ?? "Test Group Description " + Guid.NewGuid() + " Extended";

        var response = await adminClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto
        {
            Name = groupName,
            Description = groupDesc,
            Roles = []
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var groupView = await JsonSerializer.DeserializeAsync<GroupView>(
            await response.Content.ReadAsStreamAsync(),
            Json.GetJsonSerializerOptions());
        groupView.Should().NotBeNull();
        return groupView!;
    }

    public async Task<GroupView> CreateGroupWithRolesAsync(
        HttpClient adminClient,
        IEnumerable<Role> roles,
        string name,
        string description)
    {
        var group = await CreateGroupAsync(adminClient, name, description);

        foreach (var role in roles.Distinct())
        {
            var assignResp = await adminClient.PutAsync($"/api/v1/auth/groups/{group.Guid}/roles/{role}", null);
            assignResp.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        return group;
    }

    public Task<HttpResponseMessage> AssignUserToGroupAsync(
        HttpClient adminClient,
        Guid groupGuid,
        string userKcId,
        string? assigneeKcId = null)
    {
        return adminClient.PutAsJsonAsync($"/api/v1/auth/groups/{groupGuid}/users", new AssignUserToGroupDto
        {
            assigneeKcId = assigneeKcId,
            userKcId = userKcId
        });
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
