using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Auth;

[Collection(AuthCollection.Name)]
public class CreateGroupTest(AuthFixture fixture)
{
    
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

        respPutClient = await kcClient.AppApiClient.DeleteAsync($"/api/v1/auth/groups/{guid}");

        respPutClient.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var respGroupQuery = await kcClient.AppApiClient.GetAsync($"/api/v1/auth/groups/{guid}");
        respGroupQuery.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task POST_new_group_and_add_user()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);

        var respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/users", new AddUserDto
        {
            userKcId = kcClient.KcUserId
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        respPutClient = await kcClient.AppApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
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
}