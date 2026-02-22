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


        var respPutClient = await fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/groups/add", new AddGroupDto()
        {
            Name = "My Test Name" + Guid.NewGuid(),
            Description = "My Test Description,My Test Description,My Test Description" + Guid.NewGuid(),
        });

        respPutClient.StatusCode.Should().Be(HttpStatusCode.Created);
    }

}