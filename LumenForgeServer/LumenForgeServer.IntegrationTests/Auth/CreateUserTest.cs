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

        var resp = await _fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/users/add", new AddUserDto
        {
            userKcId = myKeycloakId
        });

        resp.StatusCode.Should().Be(HttpStatusCode.Created);

        var userFromDb = await _fixture.ApiClient.GetAsync($"\"/api/v1/auth/users/{myKeycloakId}");
        userFromDb.StatusCode.Should().Be(HttpStatusCode.OK);
        userFromDb.Content.Should().NotBeNull();
        
        resp = await _fixture.ApiClient.PutAsJsonAsync("/api/v1/auth/users/add", new AddUserDto
        {
            userKcId = myKeycloakId
        });

        resp.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    public Task InitializeAsync()
    { 
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}