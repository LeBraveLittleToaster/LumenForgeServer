using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Auth;

[Collection(AuthCollection.Name)]
public class AuthUserTest(AuthFixture fixture)
{
    [Fact]
    public async Task GET_users_supports_search_and_paging()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);

        var resp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users?search={kcClient.KcUserId}&limit=10&offset=0");

        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await resp.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<UserView>>(content, Json.GetJsonSerializerOptions());
        users.Should().NotBeNull();
        users.Should().Contain(u => u.UserKcId == kcClient.KcUserId);
    }

    [Fact]
    public async Task GET_users_invalid_limit_returns_bad_request()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);

        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/auth/users?limit=0&offset=0");

        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
 
    [Fact]
    public async Task POST_user_invalid_payload_returns_bad_request()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var resp = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/users", new AddUserDto
        {
            userKcId = " "
        });

        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GET_user_roles_empty_when_user_has_no_groups()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);

        var resp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}/roles");

        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await resp.Content.ReadAsStringAsync();
        var roles = JsonSerializer.Deserialize<List<Role>>(content, Json.GetJsonSerializerOptions());
        roles.Should().NotBeNull();
        roles.Should().BeEmpty();
    }

    [Fact]
    public async Task GET_user_not_found_returns_not_found()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var resp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{Guid.NewGuid()}");
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DELETE_user_removes_local_record()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);

        var deleteResp = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/users/{kcClient.KcUserId}");
        deleteResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}");
        getResp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static async Task<KcClient> CreateKcUserAndLocalUserAsync(AuthFixture fixture)
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var respDelete = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/users/{kcClient.KcUserId}");
        respDelete.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);

        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/users", new AddUserDto
        {
            userKcId = kcClient.KcUserId
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        return kcClient;
    }
}
