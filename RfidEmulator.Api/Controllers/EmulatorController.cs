using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RfidEmulator.Api.Services;
using RfidEmulator.Repository;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EmulatorController(IEmulatorManager emulatorManager, RepositoryContext context) : ControllerBase
{
    [HttpPost("Start")]
    public IActionResult Start(Guid id, CancellationToken token)
    {
        try
        {
            var reader = context.Readers.AsNoTracking()
                .Include(x => x.Antennas)
                .Include(x => x.Config)
                .SingleOrDefault(x => x.Id == id);
            
            if(reader == null) return NotFound();
            
            emulatorManager.Start(reader, token);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("Stop")]
    public IActionResult Stop(Guid id, CancellationToken token)
    {
        emulatorManager.Stop(id, token);
        
        return Ok();
    }
}