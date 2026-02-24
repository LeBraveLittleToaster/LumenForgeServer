using System.Text.Json;
using LumenForgeServer.IntegrationTests.Utils;

namespace LumenForgeServer.IntegrationTests.Client;

public sealed class KeycloakTestClient(HttpClient http)
{
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

        using var res = await http.PostAsync(tokenUrl, new FormUrlEncodedContent(fields), ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Keycloak token request failed: {(int)res.StatusCode} {res.ReasonPhrase}\n{body}");
        
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }
}