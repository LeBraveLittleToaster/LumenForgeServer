using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LumenForgeServer.Auth.Dto;
using LumenForgeServer.Common.Database;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Auth;

[Collection(AuthCollection.Name)]
public class CreateUserTest : IAsyncLifetime
{
    private readonly AuthFixture _fixture;
    private readonly AppDbContext _dbContext;

    public CreateUserTest(AuthFixture fixture, AppDbFixture dbFixture)
    {
        _fixture = fixture;
        _dbContext = dbFixture.CreateDbContext();
    }


    [Fact]
    public async Task POST_new_user_creates_user()
    {
        var myKeycloakId = _fixture.AccessToken.Claims.First(c => c.Type == "sub").Value;
        
        var respDelete = await _fixture.ApiClient.DeleteAsync($"/api/v1/auth/users/{myKeycloakId}");
        
        respDelete.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        
        var respPut1 = await _fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/users/add", new AddUserDto
        {
            userKcId = myKeycloakId
        });

        respPut1.StatusCode.Should().Be(HttpStatusCode.Created);

        var userFromDb = await _fixture.ApiClient.GetAsync($"/api/v1/auth/users/{myKeycloakId}");
        userFromDb.StatusCode.Should().Be(HttpStatusCode.OK);
        userFromDb.Content.Should().NotBeNull();
        
        var respPut2 = await _fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/users/add", new AddUserDto
        {
            userKcId = myKeycloakId
        });

        respPut2.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    public async Task InitializeAsync()
    { 
        
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}