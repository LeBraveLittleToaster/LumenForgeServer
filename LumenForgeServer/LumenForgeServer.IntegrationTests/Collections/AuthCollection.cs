using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Collections;

/// <summary>
/// xUnit collection definition that shares API host + auth provisioning via a single fixture.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class AuthCollection : ICollectionFixture<AuthFixture>
{
    public const string Name = "AuthCollection";
}