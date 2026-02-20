

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.IntegrationTests.Client;

namespace LumenForgeServer.IntegrationTests;

[Collection(AuthCollection.Name)]
public class CreateUserTest(AuthFixture fixture)
{
    private readonly AuthFixture _fixture = fixture;
    
    [Fact]
    public async Task POST_new_user_creates_user()
    {
        var myKeycloakId = _fixture.AccessToken.Claims.First(c => c.Type == "sub").Value;
        
        var resp = await _fixture.ApiClient.PostAsJsonAsync("/api/v1/user/add", new AddUserDto
        {
            keycloakId = myKeycloakId
        });

        resp.StatusCode.Should().Be(HttpStatusCode.Created);
        
        resp = await _fixture.ApiClient.PostAsJsonAsync("/api/v1/user/add", new AddUserDto
        {
            keycloakId = myKeycloakId
        });

        resp.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
}