using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Collections;

/// <summary>
/// xUnit collection definition that shares auth and database fixtures.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class AuthCollection : ICollectionFixture<AuthFixture>,ICollectionFixture<AppDbFixture>
{
    public const string Name = "AuthCollection";
}
