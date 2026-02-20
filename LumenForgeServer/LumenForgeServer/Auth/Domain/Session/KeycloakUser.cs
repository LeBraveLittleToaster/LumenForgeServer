using System.Security.Claims;

namespace LumenForgeServer.Auth.Domain.Session
{
    /// <summary>
    /// Resolves the current Keycloak user id from the HTTP context.
    /// </summary>
    public class KeycloakUser : IKeycloakUser
    {
        /// <summary>
        /// Keycloak subject identifier ("sub") for the current user, or <c>null</c> when unavailable.
        /// </summary>
        public string? UserId { get; }

        /// <summary>
        /// Initializes the accessor using the current HTTP context.
        /// </summary>
        /// <param name="accessor">HTTP context accessor used to resolve the current user.</param>
        public KeycloakUser(IHttpContextAccessor accessor)
        {
            UserId = accessor.HttpContext?.User.FindFirstValue("sub");
        }
    }
}
