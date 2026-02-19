using System.Text.Json;
using LumenForgeServer.IntegrationTests.Utils;

public sealed class KeycloakTestClient
{
    private readonly HttpClient _http;

    public KeycloakTestClient(HttpClient http) => _http = http;

    public async Task<string> GetAccessTokenPasswordGrantAsync(
        KeycloakOptions o,
        CancellationToken ct = default)
    {
        var tokenUrl = $"{o.BaseUrl.TrimEnd('/')}/realms/{o.Realm}/protocol/openid-connect/token";

        var fields = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"]  = o.ClientId,
            ["username"]   = o.Username,
            ["password"]   = o.Password
        };
        
        if (!string.IsNullOrWhiteSpace(o.ClientSecret))
            fields["client_secret"] = o.ClientSecret;

        using var res = await _http.PostAsync(tokenUrl, new FormUrlEncodedContent(fields), ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Keycloak token request failed: {(int)res.StatusCode} {res.ReasonPhrase}\n{body}");
        
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }
}