using Microsoft.AspNetCore.Mvc;
using RfidEmulator.Api.Services;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class OptimizationController(IOptimizationService optimizationService) : ControllerBase
{
    [HttpPost("/Start")]
    public async Task<IActionResult> Start(CancellationToken cancellationToken = default)
    {
        await optimizationService.Start(cancellationToken);

        return Ok();
    }

    [HttpPost("/Stop")]
    public async Task<IActionResult> Stop(CancellationToken cancellationToken = default)
    {
        await optimizationService.Stop(cancellationToken);

        return Ok();
    }
}