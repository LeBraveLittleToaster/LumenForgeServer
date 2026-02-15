using System.Security.Claims;

namespace LumenForgeServer.Common.Auth
{
    public class CurrentUser : ICurrentUser
    {
        public string? UserId { get; }

        public CurrentUser(IHttpContextAccessor accessor)
        {
            UserId = accessor.HttpContext?.User.FindFirstValue("sub");
        }
    }
}
