using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LumenForgeServer.IntegrationTests.Client;

public sealed class KcClient
{
    private KcClient(string kcUserId, TestUserInfo testUserInfo, KcOptions kcOptions)
    {
        KcUserId = kcUserId;
        TestUserInfo = testUserInfo;
        AppApiClient = new HttpClient
        {
            BaseAddress = new Uri(kcOptions.AppBaseUrl)
        };
    }

    public HttpClient AppApiClient { get; init; }

    public string KcUserId { get; set; }
    private JwtSecurityToken AccessToken { get; set; }
    private string AccessTokenString { get; set; }
    private TestUserInfo TestUserInfo { get; }

    public static async Task<KcClient> GenerateKcClientWithAccessToken(string kcUserId, TestUserInfo testUserInfo, KcOptions kcOptions)
    {
        var tokenClient = new KcClient(kcUserId, testUserInfo, kcOptions);
        await tokenClient.GetAccessTokenPasswordGrantAsync(kcOptions);
        return tokenClient;
    }

    private async Task GetAccessTokenPasswordGrantAsync(
        KcOptions o,
        CancellationToken ct = default)
    {
        var tokenUrl = $"{o.KcBaseUrl.TrimEnd('/')}/realms/{o.KcRealm}/protocol/openid-connect/token";

        var fields = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = o.KcClientId,
            ["username"] = TestUserInfo.Username,
            ["password"] = TestUserInfo.Password
        };

        using var res = await AppApiClient.PostAsync(tokenUrl, new FormUrlEncodedContent(fields), ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Keycloak token request failed: {(int)res.StatusCode} {res.ReasonPhrase}\n{body}");

        using var doc = JsonDocument.Parse(body);
        AccessTokenString = doc.RootElement.GetProperty("access_token").GetString()!;
        AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(AccessTokenString);

        AppApiClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", AccessTokenString);
    }
}