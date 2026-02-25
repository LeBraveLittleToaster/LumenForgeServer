using LumenForgeServer.IntegrationTests.Client;
using LumenForgeServer.IntegrationTests.Fixtures;

namespace LumenForgeServer.IntegrationTests.Collections;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class AuthCollection : ICollectionFixture<AuthFixture>,ICollectionFixture<AppDbFixture>
{
    public const string Name = "AuthCollection";
}
