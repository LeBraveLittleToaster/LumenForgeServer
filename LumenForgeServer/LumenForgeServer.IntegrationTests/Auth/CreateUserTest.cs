using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;

namespace LumenForgeServer.IntegrationTests.Auth;

[Collection(AuthCollection.Name)]
public class CreateUserTest(AuthFixture fixture)
{
    [Fact]
    public async Task POST_new_user_creates_user()
    {
        var myKeycloakId = fixture.AccessToken.Claims.First(c => c.Type == "sub").Value;

        var respDelete = await fixture.ApiClient.DeleteAsync($"/api/v1/auth/users/{myKeycloakId}");

        respDelete.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);

        var respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/users/add", new AddUserDto
        {
            userKcId = myKeycloakId
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);

        var userFromDb = await fixture.ApiClient.GetAsync($"/api/v1/auth/users/{myKeycloakId}");
        userFromDb.StatusCode.Should().Be(HttpStatusCode.OK);
        userFromDb.Content.Should().NotBeNull();

        var respPutTheSameClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/users/add", new AddUserDto
        {
            userKcId = myKeycloakId
        });

        respPutTheSameClient.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var getClient = await fixture.ApiClient.GetAsync($"/api/v1/auth/users/{myKeycloakId}");
        getClient.StatusCode.Should().Be(HttpStatusCode.OK);
        getClient.Content.Should().NotBeNull();
        var contentStr = await getClient.Content.ReadAsStringAsync();

        var user = JsonSerializer.Deserialize<UserView>(contentStr);
        user.Should().NotBeNull();
    }
}