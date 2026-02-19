using LumenForgeServer.Auth.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Common.Health;


[Route("api/v1/")]
[ApiController]
public class HealthController
{
    [Produces("application/json")]
    [HttpGet("health")]
    public ActionResult<IEnumerable<string>> GetHealth()
    {
        var statusAggregate = new Dictionary<string, string>
        {
            ["status"] = "healthy",
        };
        return new JsonResult(statusAggregate);
    }
    
    [Authorize(Roles = "REALM_USER")]
    [Produces("application/json")]
    [HttpGet("health2")]
    public ActionResult<IEnumerable<string>> GetHealth2()
    {
        var statusAggregate = new Dictionary<string, string>
        {
            ["status"] = "healthy",
        };
        return new JsonResult(statusAggregate);
    }
    
    [Authorize(Roles = "REALM_WORKER")]
    [Produces("application/json")]
    [HttpGet("health3")]
    public ActionResult<IEnumerable<string>> GetHealth3()
    {
        var statusAggregate = new Dictionary<string, string>
        {
            ["status"] = "healthy",
        };
        return new JsonResult(statusAggregate);
    }
}