namespace LumenForgeServer.IntegrationTests.TestSupport;

/// <summary>
/// Configuration settings used by integration test clients.
/// </summary>
public sealed class TestAuthOptions
{
    public string AppBaseUrl { get; init; } = "https://localhost:7217";
    public string KcBaseUrl { get; init; } = "http://localhost:8080";
    public string KcRealm { get; init; } = "lumenforge-realm";
    public string KcClientId { get; init; } = "lumenforge-test";

    private TestAuthOptions()
    {
    }

    public static TestAuthOptions FromEnvironment()
    {
        return new TestAuthOptions
        {
            AppBaseUrl = Environment.GetEnvironmentVariable("APP_BASEURL") ?? "https://localhost:7217",
            KcBaseUrl = Environment.GetEnvironmentVariable("KC_BASEURL") ?? "http://localhost:8080",
            KcRealm = Environment.GetEnvironmentVariable("KC_REALM") ?? "lumenforge-realm",
            KcClientId = Environment.GetEnvironmentVariable("KC_CLIENTID") ?? "lumenforge-test"
        };
    }
}
