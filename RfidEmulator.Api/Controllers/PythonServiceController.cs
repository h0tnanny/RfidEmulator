using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using RfidEmulator.Api.Configurations;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class PythonServiceController : ControllerBase
{
    private readonly IOptions<PythonService> _pythonService;

    public PythonServiceController(IOptions<PythonService> pythonService)
    {
        _pythonService = pythonService;
    }
    
    [HttpPost("SendData")]
    public async Task<IActionResult> SendData(OptimizationShortConfig config, CancellationToken token = default)
    {
        var client = new RestClient(_pythonService.Value.HostServer);
        var request = new RestRequest(_pythonService.Value.EndpointName, Method.Post);
        request.AddJsonBody(config);
        
        var response = await client.ExecuteAsync(request, token);

        return Ok(response);
    }
}