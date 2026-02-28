using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using LumenForgeServer.Auth.Client;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Common.Exceptions;
using LumenForgeServer.IntegrationTests.Client;

namespace LumenForgeServer.Auth.Service;

public class KcService
{
    private KcClient? _kcClient;
    private readonly KcClientOptions _kcOptions = KcClientOptions.FromEnvironment();
    private readonly SemaphoreSlim _lock = new(1,1);

    private async Task EnsureInitializedAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_kcClient == null)
                _kcClient = await KcClient.GenerateKcClientWithAccessTokenAsync(_kcOptions, CancellationToken.None);
            else if (_kcClient.IsTokenExpired())
            {
                await _kcClient.RefreshTokenAsync();
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<string> AddUserToKeycloak(AddKcUserDto dto, CancellationToken ct)
    {
        await EnsureInitializedAsync();
        
        var newUser = new
        {
            username = dto.Username,
            enabled = true,
            firstName = dto.FirstName,
            lastName = dto.LastName,
            email = dto.Email,
            emailVerified = true,
            groups = dto.Groups,
            realmRoles = dto.RealmRoles,
            credentials = new[]
            {
                new { type = "password", value = dto.Password, temporary = false }
            }
        };
        
        var response = await _kcClient!.AdminClient.PostAsJsonAsync($"/admin/realms/{_kcOptions.KcRealm}/users", newUser, ct);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            if (response.StatusCode == HttpStatusCode.Conflict)
                throw new UniqueConstraintException("User already exists in Keycloak.", new Exception(body));

            throw new KeycloakException($"Failed to create user: Http {(int)response.StatusCode} {body}");
        }

        var location = response.Headers.Location?.ToString();

        if (string.IsNullOrEmpty(location))
            throw new KeycloakException("User created but Location header missing.");

        var userId = location.Split('/').Last();

        return userId;
    }
}
