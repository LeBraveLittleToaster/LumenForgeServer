using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LumenForgeServer.IntegrationTests.Client;

public class KeycloakAdminClient
{
    private static HttpClient _httpClient = new();
    private KcOptions _kcOptions;

    private string? _adminToken = null;
    public KeycloakAdminClient(KcOptions kcOptions)
    {
        _kcOptions = kcOptions;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(kcOptions.KeycloakOptions.BaseUrl);
    }

    public async Task<TestUserLogin> CreateKcUserAsync(string username, string password, string email, string firstName, string lastName, string[] groups, string[] realmRoles, CancellationToken ct)
    {
        if (_adminToken == null)
        {
            await RequestAdminToken(CancellationToken.None);
        }
        var newUser = new
        {
            username = username,
            enabled = true,
            firstName = firstName,
            lastName = lastName,
            email = email,
            emailVerified = true,
            groups = groups,
            realmRoles = realmRoles,
            credentials = new[]
            {
                new { type = "password", value = password, temporary = false }
            }
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        
        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{_kcOptions.KeycloakOptions.Realm}/users", newUser);
        
        var httpStatus = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"User created successfully. Status: {httpStatus}");
            var body = await response.Content.ReadAsStringAsync(ct);
            var respJson = JsonSerializer.Deserialize<JsonElement>(body);
            Console.WriteLine(respJson.GetProperty("access_token").GetString());
            return new TestUserLogin(username, password);
        }
        else
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed. Status: {httpStatus}, Error: {errorBody}");
            throw new Exception(errorBody);
        }
    }
    
    private async Task<bool> RequestAdminToken(CancellationToken ct)
    {
        var data = new Dictionary<string, string>
        {
            ["username"] = _kcOptions.AdminUser,
            ["password"] = _kcOptions.AdminPass,
            ["grant_type"] = "password",
            ["client_id"] = "admin-cli",
        };

        using var content = new FormUrlEncodedContent(data);

        try
        {
            var url = $"{_kcOptions.KeycloakOptions.BaseUrl}/realms/{_kcOptions.AdminRealm}/protocol/openid-connect/token";
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