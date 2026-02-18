using System.Security.Claims;

namespace LumenForgeServer.Auth.Domain.Session
{
    public class KeycloakUser : IKeycloakUser
    {
        public string? UserId { get; }

        public KeycloakUser(IHttpContextAccessor accessor)
        {
            UserId = accessor.HttpContext?.User.FindFirstValue("sub");
        }
    }
}
