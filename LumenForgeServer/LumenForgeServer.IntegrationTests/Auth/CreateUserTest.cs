using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Auth;

/// <summary>
/// Integration tests for user creation behavior in the auth API.
/// </summary>
[Collection(AuthCollection.Name)]
public class CreateUserTest(AuthFixture fixture)
{
    [Fact]
    public async Task GET_initial_admin_user_returns_token()
    {
        var options = KcAndAppClientOptions.FromEnvironment();
        var adminBundle = await fixture.GetInitialAdminUserAsync(options);
        adminBundle.Should().NotBeNull();
        adminBundle.GetKcUserId().Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task POST_new_user_creates_user()
    {
        var options = KcAndAppClientOptions.FromEnvironment();

        var testUser = CreateTestUserDto.CreateTestUser();
        var userBundle = await fixture.CreateNewUserWithRolesAsync(options, testUser, [Role.UserRead]);
        var userKcId = userBundle.GetKcUserId();

        var userFromDb = await userBundle.AppClient.GetAsync($"/api/v1/auth/users/{userKcId}");
        userFromDb.StatusCode.Should().Be(HttpStatusCode.OK);
        userFromDb.Content.Should().NotBeNull();

        var respPutTheSameClient = await userBundle.AppClient.PutAsJsonAsync(
            "/api/v1/auth/users",
            testUser);

        respPutTheSameClient.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var getClient = await userBundle.AppClient.GetAsync($"/api/v1/auth/users/{userKcId}");
        getClient.StatusCode.Should().Be(HttpStatusCode.OK);
        getClient.Content.Should().NotBeNull();
        var contentStr = await getClient.Content.ReadAsStringAsync();
        
        var user = JsonSerializer.Deserialize<UserView>(contentStr, Json.GetJsonSerializerOptions());
        user.Should().NotBeNull();
    }
}
