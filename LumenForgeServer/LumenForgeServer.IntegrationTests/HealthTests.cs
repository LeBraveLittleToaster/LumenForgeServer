using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests;

[Collection(AuthCollection.Name)]
public class HealthTests(AuthFixture fixture)
{
    
    private readonly AuthFixture _fixture = fixture;

    [Fact]
    public async Task GET_health_returns_ok()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(new TestUserInfo("tu", "pw", "t@t.de", "TV","TN", ["users"], ["REALM_ADMIN"]), CancellationToken.None);
        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/health");

        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        json.Should().ContainKey("status");
    }
    
    [Fact]
    public async Task GET_health2_returns_ok()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(new TestUserInfo("tu", "pw", "t@t.de", "TV","TN", ["users"], ["REALM_ADMIN"]), CancellationToken.None);
        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/health2");

        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        json.Should().ContainKey("status");
    }
    
    [Fact]
    public async Task GET_health3_returns_ok()
    {
        var kcClient = await fixture.CreateNewTestUserClientAsync(new TestUserInfo("tu", "pw", "t@t.de", "TV","TN", ["users"], ["REALM_ADMIN"]), CancellationToken.None);
        var resp = await kcClient.AppApiClient.GetAsync("/api/v1/health3");

        resp.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
