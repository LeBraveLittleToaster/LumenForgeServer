namespace LumenForgeServer.IntegrationTests.Client;

/// <summary>
/// Immutable test user payload used to provision Keycloak identities.
/// </summary>
public record TestUserInfo(
    string Username,
    string Password,
    string Email,
    string FirstName,
    string LastName,
    string[] Groups,
    string[] RealmRoles
)
{
    public static TestUserInfo CreateTestUserInfoWithGuid()
    {
        var guid = Guid.NewGuid().ToString();

        return new TestUserInfo(
            Username: "TestUser" + guid,
            Password: "Password" + guid,
            Email: "test-email-" + guid + "@test.de",
            FirstName: "TestFirstName" + guid,
            LastName: "TestLastName" + guid,
            Groups: ["admins"],
            RealmRoles: []
        );
    }
}
