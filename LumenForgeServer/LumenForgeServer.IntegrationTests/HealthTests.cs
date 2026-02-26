using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests;

/// <summary>
/// Integration tests for public and role-restricted health endpoints.
/// </summary>
[Collection(AuthCollection.Name)]
public class HealthTests(AuthFixture fixture)
{

    [Fact]
    public async Task GET_health_returns_ok()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(TestUserInfo.CreateTestUserInfoWithGuid(), CancellationToken.None);
        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/health");

        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        json.Should().ContainKey("status");
    }
}
