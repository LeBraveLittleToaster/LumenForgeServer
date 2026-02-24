using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Utils;

namespace LumenForgeServer.IntegrationTests.Auth;

public class AssignUsersToGroupTests
{
    [Fact]
    public async Task AssignUsersToGroup()
    {
        var kcOptions = KcOptions.FromEnvironment();
        var adminClient = new KeycloakAdminClient(kcOptions);
        var guid =  Guid.NewGuid().ToString();
        
        var userRecord = await adminClient.CreateKcUserAsync(
            "testuser" + guid,
            "testtest",
            $"test{guid}@test.de",
            $"Test{guid}",
            $"User{guid}",
            ["users"],
            ["REALM_ADMIN"],
            CancellationToken.None);
        
        var httpClient = new HttpClient();
        var testClient = new KeycloakTestClient(httpClient);
        // TODO: Merge testclient and adminclient
    }
}