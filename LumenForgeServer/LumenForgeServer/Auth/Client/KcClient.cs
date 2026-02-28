using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;
using LumenForgeServer.IntegrationTests.Client;
using NodaTime;

namespace LumenForgeServer.Auth.Client;

/// <summary>
/// Authenticated app client backed by a Keycloak access token for integration tests.
/// </summary>
public sealed class KcClient
{
    private readonly KcClientOptions _kcOptions;

    private KcClient(KcClientOptions kcOptions)
    {
        _kcOptions = kcOptions;
        AdminClient = new HttpClient
        {
            BaseAddress = new Uri(kcOptions.KcBaseUrl)
        };
    }

    public HttpClient AdminClient { get; init; }
    private JwtSecurityToken? AccessToken { get; set; }
    private string? AccessTokenString { get; set; }

    public static async Task<KcClient> GenerateKcClientWithAccessTokenAsync(KcClientOptions kcClientOptions, CancellationToken ct)
    {
        var kcClient = new KcClient(kcClientOptions);
        await kcClient.RequestAndAttachAdminTokenAsync(kcClientOptions, ct);
        return kcClient;
    }
    
    private async Task<bool> RequestAndAttachAdminTokenAsync(KcClientOptions kcClientOptions, CancellationToken ct)
    {
        var data = new Dictionary<string, string>
        {
            ["username"] = kcClientOptions.KcAdminUser,
            ["password"] = kcClientOptions.KcAdminPass,
            ["grant_type"] = "password",
            ["client_id"] = "admin-cli",
        };

        using var content = new FormUrlEncodedContent(data);

        try
        {
            var url = $"/realms/{kcClientOptions.KcAdminRealm}/protocol/openid-connect/token";
            var resp = await AdminClient.PostAsync(url, content, ct);

            var body = await resp.Content.ReadAsStringAsync(ct);
            var respJson = JsonSerializer.Deserialize<JsonElement>(body);
            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Keycloak token request failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");

        
            var adminToken = respJson.GetProperty("access_token").GetString();
            
            AccessTokenString = respJson.GetProperty("access_token").GetString()!;
            AccessToken = new JwtSecurityTokenHandler().ReadJwtToken(AccessTokenString);
            
            if (adminToken != null)
            {
                AdminClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", AccessTokenString);
                return true;
            }
         
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }
    
    public async Task RefreshTokenAsync(CancellationToken ct = default)
    {
        await RequestAndAttachAdminTokenAsync(_kcOptions, ct);
    }
    
    public bool IsTokenExpired(Duration? skew = null)
    {
        if (AccessToken is null) return true;

        var nowUtc = SystemClock.Instance.GetCurrentInstant().ToDateTimeUtc();
        var buffer = (skew ?? Duration.FromMinutes(1)).ToTimeSpan();
        return AccessToken.ValidTo <= nowUtc.Add(buffer);
    }
}
