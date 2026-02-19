namespace LumenForgeServer.IntegrationTests.Utils;

public sealed class KeycloakOptions
{
        public required string BaseUrl { get; init; }     // http://localhost:8080 (or include /auth if needed)
        public required string Realm { get; init; }       // lumenforge-realm
        public required string ClientId { get; init; }    // lumenforge-test
        public string? ClientSecret { get; init; }        // set if confidential
        public required string Username { get; init; }    // alice
        public required string Password { get; init; }    // alice123
    
}