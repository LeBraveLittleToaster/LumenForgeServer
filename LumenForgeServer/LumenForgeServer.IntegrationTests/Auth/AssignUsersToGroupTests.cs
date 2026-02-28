using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Domain;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Command;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;
using LumenForgeServer.IntegrationTests.TestSupport;

namespace LumenForgeServer.IntegrationTests.Auth;

/// <summary>
/// Integration tests for group membership and role assignment endpoints.
/// </summary>
[Collection(AuthCollection.Name)]
public class AssignUsersToGroupTests(AuthFixture fixture)
{
    [Fact]
    public async Task Assign_user_to_group_and_query_membership()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var respAssignUser = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{group.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = kcClient.KcUserId
        });

        respAssignUser.StatusCode.Should().Be(HttpStatusCode.OK);

        var groupUsersResp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups/{group.Guid}/users");
        groupUsersResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var groupUsers = JsonSerializer.Deserialize<List<UserView>>(await groupUsersResp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        groupUsers.Should().NotBeNull();
        groupUsers.Should().Contain(u => u.UserKcId == kcClient.KcUserId);

        var userGroupsResp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}/groups");
        userGroupsResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var userGroups = JsonSerializer.Deserialize<List<GroupView>>(await userGroupsResp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        userGroups.Should().NotBeNull();
        userGroups.Should().Contain(g => g.Guid == group.Guid);
    }

    [Fact]
    public async Task Assign_user_to_group_twice_returns_conflict()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var firstAssign = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{group.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = kcClient.KcUserId
        });
        firstAssign.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondAssign = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{group.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = kcClient.KcUserId
        });
        secondAssign.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Remove_user_from_group_then_not_listed()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var assignResp = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{group.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = kcClient.KcUserId
        });
        assignResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var removeResp = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/groups/{group.Guid}/users/{kcClient.KcUserId}");
        removeResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var groupUsersResp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups/{group.Guid}/users");
        groupUsersResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var groupUsers = JsonSerializer.Deserialize<List<UserView>>(await groupUsersResp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        groupUsers.Should().NotBeNull();
        groupUsers.Should().NotContain(u => u.UserKcId == kcClient.KcUserId);

        var removeAgainResp = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/groups/{group.Guid}/users/{kcClient.KcUserId}");
        removeAgainResp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Assign_role_to_group_and_query_roles()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var assignRoleResp = await kcClient.AppApiClient.PutAsync($"/api/v1/auth/groups/{group.Guid}/roles/{Role.CategoryRead}", null);
        assignRoleResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var rolesResp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups/{group.Guid}/roles");
        rolesResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var roles = JsonSerializer.Deserialize<List<Role>>(await rolesResp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        roles.Should().NotBeNull();
        roles.Should().Contain(Role.CategoryRead);
    }

    [Fact]
    public async Task User_roles_include_group_roles()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var assignRoleResp = await kcClient.AppApiClient.PutAsync($"/api/v1/auth/groups/{group.Guid}/roles/{Role.DeviceRead}", null);
        assignRoleResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var assignUserResp = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{group.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = kcClient.KcUserId
        });
        assignUserResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var userRolesResp = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/users/{kcClient.KcUserId}/roles");
        userRolesResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var roles = JsonSerializer.Deserialize<List<Role>>(await userRolesResp.Content.ReadAsStringAsync(), Json.GetJsonSerializerOptions());
        roles.Should().NotBeNull();
        roles.Should().Contain(Role.DeviceRead);
    }

    [Fact]
    public async Task Assign_role_twice_returns_conflict()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var firstAssign = await kcClient.AppApiClient.PutAsync($"/api/v1/auth/groups/{group.Guid}/roles/{Role.DeviceRead}", null);
        firstAssign.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var secondAssign = await kcClient.AppApiClient.PutAsync($"/api/v1/auth/groups/{group.Guid}/roles/{Role.DeviceRead}", null);
        secondAssign.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Remove_role_not_assigned_returns_not_found()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var removeResp = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/groups/{group.Guid}/roles/{Role.DeviceUpdate}");
        removeResp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Assign_invalid_role_returns_bad_request()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var resp = await kcClient.AppApiClient.PutAsync($"/api/v1/auth/groups/{group.Guid}/roles/NotARole", null);
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Assign_user_not_found_returns_not_found()
    {
        var kcClient = await CreateKcUserAndLocalUserAsync(fixture);
        var group = await CreateGroupAsync(kcClient);

        var resp = await kcClient.AppApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{group.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = Guid.NewGuid().ToString()
        });

        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static Task<TestAppClient> CreateKcUserAndLocalUserAsync(AuthFixture fixture)
    {
        return null;
    }

    private static async Task<GroupView> CreateGroupAsync(TestAppClient kcClient)
    {
        var guid = Guid.NewGuid();
        var groupName = "Test Group " + guid;
        var groupDesc = "Test Group Description " + guid + " Extended";

        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto
        {
            Name = groupName,
            Description = groupDesc
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);

        var groupView = await JsonSerializer.DeserializeAsync<GroupView>(await respPutClient.Content.ReadAsStreamAsync(), Json.GetJsonSerializerOptions());
        groupView.Should().NotBeNull();
        return groupView!;
    }
}
