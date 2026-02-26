using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LumenForgeServer.IntegrationTests.Client;

/// <summary>
/// Minimal Keycloak admin client for creating test users during integration tests.
/// </summary>
public class KcAdminClient
{
    private static HttpClient _httpClient = new();
    private readonly KcOptions _kcOptions;

    private string? _adminToken = null;
    public KcAdminClient(KcOptions kcOptions)
    {
        _kcOptions = kcOptions;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(kcOptions.KcBaseUrl);
    }

    public async Task<string?> CreateKcUserAsync(TestUserInfo testUserInfo, CancellationToken ct)
    {
        if (_adminToken == null)
        {
            await RequestAdminToken(ct);
        }
        var newUser = new
        {
            username = testUserInfo.Username,
            enabled = true,
            firstName = testUserInfo.FirstName,
            lastName = testUserInfo.LastName,
            email = testUserInfo.Email,
            emailVerified = true,
            groups = testUserInfo.Groups,
            realmRoles = testUserInfo.RealmRoles,
            credentials = new[]
            {
                new { type = "password", value = testUserInfo.Password, temporary = false }
            }
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        
        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{_kcOptions.KcRealm}/users", newUser, ct);
        
        var httpStatus = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"User created successfully. Status: {httpStatus}");
            return response.Headers.Location?.Segments[^1];
        }

        var errorBody = await response.Content.ReadAsStringAsync(ct);
        Console.WriteLine($"Failed. Status: {httpStatus}, Error: {errorBody}");
        throw new Exception(errorBody);
    }
    
    private async Task<bool> RequestAdminToken(CancellationToken ct)
    {
        var data = new Dictionary<string, string>
        {
            ["username"] = _kcOptions.KcAdminUser,
            ["password"] = _kcOptions.KcAdminPass,
            ["grant_type"] = "password",
            ["client_id"] = "admin-cli",
        };

        using var content = new FormUrlEncodedContent(data);

        try
        {
            var url = $"{_kcOptions.KcBaseUrl}/realms/{_kcOptions.KcAdminRealm}/protocol/openid-connect/token";
            var resp = await _httpClient.PostAsync(url, content);

            var body = await resp.Content.ReadAsStringAsync();
            var respJson = JsonSerializer.Deserialize<JsonElement>(body);
            resp.EnsureSuccessStatusCode();
        
            var adminToken = respJson.GetProperty("access_token").GetString();
            if (adminToken != null)
            {
                _adminToken = adminToken;
                return true;
            }
         
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }
}
