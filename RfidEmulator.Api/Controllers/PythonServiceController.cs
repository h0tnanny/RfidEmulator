using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using RfidEmulator.Api.Configurations;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class PythonServiceController(IOptions<PythonService> pythonService) : ControllerBase
{
    [HttpPost("SendData")]
    public async Task<IActionResult> SendData(OptimizationShortConfig config, CancellationToken token = default)
    {
        var options = new RestClientOptions(pythonService.Value.HostServer);
        var client = new RestClient(options);
        var request = new RestRequest(pythonService.Value.EndpointName, Method.Post);
        request.AddJsonBody(config);
        await client.PostAsync(request, token);

        return Ok();
    }
}