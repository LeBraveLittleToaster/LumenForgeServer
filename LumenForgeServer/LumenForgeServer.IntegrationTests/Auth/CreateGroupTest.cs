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
/// Integration tests for group CRUD and membership workflow behavior.
/// </summary>
[Collection(AuthCollection.Name)]
public class CreateGroupTest(AuthFixture fixture)
{
    [Fact]
    public async Task GET_groups_supports_search_and_paging()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var group = await CreateGroupAsync(kcClient);

        var resp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups?search={Uri.EscapeDataString(group.Name)}&limit=10&offset=0");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var groups = JsonSerializer.Deserialize<List<GroupView>>(await resp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        groups.Should().NotBeNull();
        groups.Should().Contain(g => g.Guid == group.Guid);
    }

    [Fact]
    public async Task GET_groups_invalid_limit_returns_bad_request()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/auth/groups?limit=0&offset=0");
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    
    [Fact]
    public async Task POST_new_group_creates_user()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        
        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        
        respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task POST_new_group_query_and_delete()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        
        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);

        var groupView = await JsonSerializer.DeserializeAsync<GroupView>(await respPutClient.Content.ReadAsStreamAsync(), Json.GetJsonSerializerOptions());
        groupView.Should().NotBeNull();

        respPutClient = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/groups/{groupView!.Guid}");

        respPutClient.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var respGroupQuery = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups/{groupView.Guid}");
        respGroupQuery.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task POST_group_invalid_payload_returns_bad_request()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var respMissingName = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto
        {
            Name = " ",
            Description = "Valid description with enough length"
        });

        respMissingName.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var respShortDesc = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto
        {
            Name = "Valid Name " + Guid.NewGuid(),
            Description = "short"
        });

        respShortDesc.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PATCH_group_updates_fields()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        var group = await CreateGroupAsync(kcClient);

        var updateResp = await kcClient.AppApiClient.PatchAsJsonAsync($"/api/v1/auth/groups/{group.Guid}", new UpdateGroupDto
        {
            Name = "Updated Name " + Guid.NewGuid(),
            Description = "Updated Description " + Guid.NewGuid() + " extended"
        });

        updateResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = JsonSerializer.Deserialize<GroupView>(await updateResp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        updated.Should().NotBeNull();
        updated!.Name.Should().StartWith("Updated Name");
        updated.Description.Should().StartWith("Updated Description");
    }

    [Fact]
    public async Task PATCH_group_empty_body_returns_bad_request()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        var group = await CreateGroupAsync(kcClient);

        var updateResp = await kcClient.AppApiClient.PatchAsJsonAsync($"/api/v1/auth/groups/{group.Guid}", new UpdateGroupDto());
        updateResp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GET_group_invalid_guid_returns_not_found()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/auth/groups/not-a-guid");
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task POST_new_group_and_add_user()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        
        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        var groupView = await JsonSerializer.DeserializeAsync<GroupView>(await respPutClient.Content.ReadAsStreamAsync(), Json.GetJsonSerializerOptions());
        groupView.Should().NotBeNull();

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);

        var groupGet = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups/{groupView.Guid}");
        groupGet.StatusCode.Should().Be(HttpStatusCode.OK);

        await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}");

        var respAssignUser = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{groupView.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = kcClient.KcUserId
        });

        respAssignUser.StatusCode.Should().Be(HttpStatusCode.OK);

    }

    private static async Task<GroupView> CreateGroupAsync(TestAppClient kcClient)
    {
        var guid = Guid.NewGuid();
        var groupName = "Test Group " + guid;
        var groupDesc = "Test Group Description " + guid + " Extended";

        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);

        var groupView = await JsonSerializer.DeserializeAsync<GroupView>(await respPutClient.Content.ReadAsStreamAsync(), Json.GetJsonSerializerOptions());
        groupView.Should().NotBeNull();
        return groupView!;
    }
}
