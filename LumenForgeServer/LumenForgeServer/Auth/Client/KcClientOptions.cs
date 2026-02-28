namespace LumenForgeServer.IntegrationTests.Client;

/// <summary>
/// Configuration settings used by Keycloak-aware integration test clients.
/// </summary>
public class KcClientOptions
{
    public string KcAdminPass { get; init; }
    public string KcAdminUser { get; init; }

    public string KcAdminRealm { get; init; }

    public string KcClientId { get; init; }

    public string KcRealm { get; init; }
    
    public string KcBaseUrl { get; init; }
    
    public string AppBaseUrl { get; init; }

    private KcClientOptions()
    {
    }

    public static KcClientOptions FromEnvironment()
    {
        return new KcClientOptions
        {
           
            KcBaseUrl = Environment.GetEnvironmentVariable("KC_BASEURL") ?? "http://localhost:8080",
            KcRealm = Environment.GetEnvironmentVariable("KC_REALM") ?? "lumenforge-realm",
            KcClientId = Environment.GetEnvironmentVariable("KC_CLIENTID") ?? "admin-cli",
            
            KcAdminRealm = Environment.GetEnvironmentVariable("KC_ADMIN_REALM") ?? "master",
            KcAdminUser = Environment.GetEnvironmentVariable("KC_ADMIN_USER") ?? "admin",
            KcAdminPass = Environment.GetEnvironmentVariable("KC_ADMIN_PASS") ?? "adminpassword"
        };
    }
}
