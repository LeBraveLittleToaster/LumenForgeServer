using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Auth;

/// <summary>
/// Integration tests for user creation behavior in the auth API.
/// </summary>
[Collection(AuthCollection.Name)]
public class CreateUserTest(AuthFixture fixture)
{
    [Fact]
    public async Task TESCHT()
    {
        await fixture.CreateNewUser();
    }
    
    [Fact]
    public async Task POST_new_user_creates_user()
    {
        var testUser = TestUserInfo.CreateTestUserInfoWithGuid();
        

        var userFromDb = await fixture.AdminClient.AdminClient.GetAsync($"/api/v1/auth/users/{testUser.Username}");
        userFromDb.StatusCode.Should().Be(HttpStatusCode.OK);
        userFromDb.Content.Should().NotBeNull();

        var respPutTheSameClient = await fixture.AdminClient.AdminClient.PutAsJsonAsync(
            "/api/v1/auth/users",
            testUser.ToAddKcUserDto());

        respPutTheSameClient.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var getClient = await fixture.AdminClient.AdminClient.GetAsync($"/api/v1/auth/users/{testUser.Username}");
        getClient.StatusCode.Should().Be(HttpStatusCode.OK);
        getClient.Content.Should().NotBeNull();
        var contentStr = await getClient.Content.ReadAsStringAsync();
        
        var user = JsonSerializer.Deserialize<UserView>(contentStr, Json.GetJsonSerializerOptions());
        user.Should().NotBeNull();
    }
}
