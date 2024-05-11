using Microsoft.AspNetCore.Mvc;
using RfidEmulator.Api.Services;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class OptimizationController(IOptimizationService optimizationService) : ControllerBase
{
    [HttpPost("/Start")]
    public IActionResult Start(CancellationToken cancellationToken = default)
    {
        optimizationService.Start(cancellationToken);

        return Ok();
    }

    [HttpPost("/Stop")]
    public IActionResult Stop(CancellationToken cancellationToken = default)
    {
        optimizationService.Stop(cancellationToken);

        return Ok();
    }
}