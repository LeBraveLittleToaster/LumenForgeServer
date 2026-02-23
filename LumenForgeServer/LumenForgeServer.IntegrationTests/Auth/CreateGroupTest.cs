using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
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
        var respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups/add", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
        
        respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups/add", new AddGroupDto()
        {
            Name = groupName,
            Description = groupDesc,
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

}