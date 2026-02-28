namespace LumenForgeServer.IntegrationTests.TestSupport;

/// <summary>
/// Authenticated app client backed by a Keycloak access token for integration tests.
/// </summary>
public sealed class TestAppClient
{
    public TestAppClient(HttpClient appApiClient, string kcUserId, TestUserInfo userInfo)
    {
        AppApiClient = appApiClient;
        KcUserId = kcUserId;
        UserInfo = userInfo;
    }

    public HttpClient AppApiClient { get; }
    public string KcUserId { get; }
    public TestUserInfo UserInfo { get; }
}
