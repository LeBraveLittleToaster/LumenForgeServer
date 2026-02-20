namespace LumenForgeServer.Auth.Domain.Session
{
    /// <summary>
    /// Abstraction for accessing the current Keycloak user id.
    /// </summary>
    public interface IKeycloakUser
    {
        /// <summary>
        /// Keycloak subject identifier ("sub") for the current user, or <c>null</c> when unavailable.
        /// </summary>
        string? UserId { get; }
    }

}
