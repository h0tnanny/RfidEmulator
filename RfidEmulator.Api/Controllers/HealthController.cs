using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
[AllowAnonymous]
public class HealthController(HealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Check(CancellationToken token)
    {
        var report = await healthCheckService.CheckHealthAsync(token);
        var result = new
        {
            status = report.Status.ToString(),
            errors = report.Entries.Select(e => 
                new { name = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description?.ToString() })
        };
        
        return report.Status == HealthStatus.Healthy ? Ok(result) : StatusCode((int)HttpStatusCode.ServiceUnavailable, result);
    }
}