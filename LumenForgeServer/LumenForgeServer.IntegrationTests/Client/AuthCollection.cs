namespace LumenForgeServer.IntegrationTests.Client;

using Xunit;

[CollectionDefinition(Name)]
public sealed class AuthCollection : ICollectionFixture<AuthFixture>
{
    public const string Name = "AuthCollection";
}
