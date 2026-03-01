using FluentAssertions;
using LumenForgeServer.IntegrationTests.Collections;
using LumenForgeServer.IntegrationTests.Fixtures;
using System.Net;

[Collection(AuthCollection.Name)]
public class HealthTests(AuthFixture fixture)
{
    [Fact]
    public async Task GET_health_returns_ok()
    {
        var resp = await fixture.AnonymousClient.GetAsync("/api/v1/health");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}