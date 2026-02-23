using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Auth.Dto.Views;
using LumenForgeServer.Common;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;

namespace LumenForgeServer.IntegrationTests.Auth;

[Collection(AuthCollection.Name)]
public class CreateGroupTest(AuthFixture fixture)
{
    [Fact]
    public async Task POST_new_group_creates_user()
    {
        var myKeycloakId = fixture.AccessToken.Claims.First(c => c.Type == "sub").Value;

        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        var respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        
        respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task POST_new_group_query_and_delete()
    {
        var myKeycloakId = fixture.AccessToken.Claims.First(c => c.Type == "sub").Value;

        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        var respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        
        respPutClient = await fixture.ApiClient.DeleteAsync($"/api/v1/auth/groups/{guid}");

        respPutClient.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var respGroupQuery = await fixture.ApiClient.GetAsync($"/api/v1/auth/groups/{guid}");
        respGroupQuery.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task POST_new_group_and_add_user()
    {
        var myKeycloakId = fixture.AccessToken.Claims.First(c => c.Type == "sub").Value;

        var guid = Guid.NewGuid();
        var groupName = "My Test Name" + guid;
        var groupDesc = "My Test Description,My Test Description,My Test Description" + guid;
        var respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        var groupView = await JsonSerializer.DeserializeAsync<GroupView>(await respPutClient.Content.ReadAsStreamAsync(), Json.GetJsonSerializerOptions());
        groupView.Should().NotBeNull();

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var groupGet = await fixture.ApiClient.GetAsync($"/api/v1/auth/groups/{groupView.Guid}");
        groupGet.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await fixture.ApiClient.GetAsync($"/api/v1/auth/users/{myKeycloakId}");
        
        var respAssignUser = await fixture.ApiClient.PutAsJsonAsync($"/api/v1/auth/groups/{groupView.Guid}/users", new AssignUserToGroupDto
        {
            userKcId = myKeycloakId,
            assigneeKcId = null
        });
        
        respAssignUser.StatusCode.Should().Be(HttpStatusCode.OK);
    }

}