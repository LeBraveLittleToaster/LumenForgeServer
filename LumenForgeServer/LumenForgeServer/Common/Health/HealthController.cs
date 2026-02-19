using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Common.Health;

[Route("api/v1/")]
[ApiController]
public class HealthController
{
    [Produces("application/json")]
    [HttpGet("health")]
    public ActionResult<IEnumerable<string>> Get()
    {
        var statusAggragte = new Dictionary<string, string>
        {
            ["status"] = "healthy",
        };
        return new JsonResult(statusAggragte);
    }
}