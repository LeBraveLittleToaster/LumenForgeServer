using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Fixtures;

/// <summary>
/// Fixture for provisioning authenticated Keycloak test clients.
/// </summary>
public sealed class AuthFixture : IAsyncLifetime
{
    public readonly TestAuthOptions Options;

    public AuthFixture()
    {
        Options = TestAuthOptions.FromEnvironment();
    }

    public async Task InitializeAsync()
    {
        
    }

    public async Task<TestAppClient> CreateNewTestUserClientAsync(TestUserInfo testUserInfo, CancellationToken ct)
    {
        var appClient = new HttpClient
        {
            BaseAddress = new Uri(Options.AppBaseUrl)
        };

        var registerResponse = await appClient.PutAsJsonAsync(
            "/api/v1/auth/users",
            testUserInfo.ToAddKcUserDto(),
            ct);

        if (registerResponse.StatusCode != HttpStatusCode.Created)
        {
            var body = await registerResponse.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException(
                $"Register endpoint failed: {(int)registerResponse.StatusCode} {registerResponse.ReasonPhrase}\n{body}");
        }

        var kcUserId = await ReadUserKcIdAsync(registerResponse, ct);
        var accessToken = await RequestAccessTokenAsync(appClient, testUserInfo, Options, ct);

        appClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        return new TestAppClient(appClient, kcUserId, testUserInfo);
    }

    private static async Task<string> ReadUserKcIdAsync(HttpResponseMessage response, CancellationToken ct)
    {
        var body = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(body);

        if (!doc.RootElement.TryGetProperty("userKcId", out var kcUserIdElement))
            throw new InvalidOperationException("Register endpoint response missing userKcId.");

        var kcUserId = kcUserIdElement.GetString();
        if (string.IsNullOrWhiteSpace(kcUserId))
            throw new InvalidOperationException("Register endpoint returned empty userKcId.");

        return kcUserId;
    }

    private static async Task<string> RequestAccessTokenAsync(
        HttpClient httpClient,
        TestUserInfo testUserInfo,
        TestAuthOptions options,
        CancellationToken ct)
    {
        var tokenUrl = $"{options.KcBaseUrl.TrimEnd('/')}/realms/{options.KcRealm}/protocol/openid-connect/token";

        var fields = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = options.KcClientId,
            ["username"] = testUserInfo.Username,
            ["password"] = testUserInfo.Password
        };

        using var res = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(fields), ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Keycloak token request failed: {(int)res.StatusCode} {res.ReasonPhrase}\n{body}");

        using var doc = JsonDocument.Parse(body);
        if (!doc.RootElement.TryGetProperty("access_token", out var tokenElement))
            throw new InvalidOperationException("Keycloak token response missing access_token.");

        var accessToken = tokenElement.GetString();
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new InvalidOperationException("Keycloak token response returned empty access_token.");

        return accessToken;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
