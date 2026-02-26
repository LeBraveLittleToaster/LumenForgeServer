using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Auth;

/// <summary>
/// Integration tests for user creation behavior in the auth API.
/// </summary>
[Collection(AuthCollection.Name)]
public class CreateUserTest(AuthFixture fixture)
{
    [Fact]
    public async Task POST_new_user_creates_user()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var respDelete = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/users/{kcClient.KcUserId}");

        respDelete.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);

        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/users", new AddUserDto
        {
            userKcId = kcClient.KcUserId
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);

        var userFromDb = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}");
        userFromDb.StatusCode.Should().Be(HttpStatusCode.OK);
        userFromDb.Content.Should().NotBeNull();

        var respPutTheSameClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/users", new AddUserDto
        {
            userKcId = kcClient.KcUserId
        });

        respPutTheSameClient.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var getClient = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}");
        getClient.StatusCode.Should().Be(HttpStatusCode.OK);
        getClient.Content.Should().NotBeNull();
        var contentStr = await getClient.Content.ReadAsStringAsync();
        
        var user = JsonSerializer.Deserialize<UserView>(contentStr, Json.GetJsonSerializerOptions());
        user.Should().NotBeNull();
    }
}
