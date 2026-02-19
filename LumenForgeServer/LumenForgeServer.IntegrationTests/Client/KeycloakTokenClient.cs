namespace LumenForgeServer.IntegrationTests.Utils;

using System.Net.Http.Headers;
using System.Text.Json;

public static class KeycloakTokenClient
{
    public static async Task<string> GetClientCredentialsTokenAsync(
        HttpClient http,
        string tokenEndpoint,
        string clientId,
        string clientSecret)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
        });

        using var resp = await http.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        await using var stream = await resp.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }

    public static async Task<string> GetPasswordTokenAsync(
        HttpClient http,
        string tokenEndpoint,
        string clientId,
        string clientSecret,
        string username,
        string password)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["username"] = username,
                ["password"] = password,
            })
        };

        using var resp = await http.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }
}
