using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RfidEmulator.Api.Services;
using RfidEmulator.Domain.DTOs;
using RfidEmulator.Domain.Entity;
using RfidEmulator.Repository;

namespace RfidEmulator.Api.Controllers;

public class ReaderController(ILogger<ReaderController> logger, RepositoryContext context, IMapper mapper,
    IEmulatorManager emulatorManager) :
    BaseController<ReaderController>(logger, context, mapper)
{
    [HttpGet("GetReaders")]
    [ProducesResponseType<IEnumerable<Reader>>(200)]
    public async Task<IActionResult> GetReaders()
    {
        var readers = await context.Readers.AsNoTracking()
            .Include(x => x.Config)
            .Include(x => x.Antennas)
            .ToListAsync();

        var emulators = emulatorManager.GetEmulators();
        var result = readers.Select(x => new
        {
            Reader = x,
            IsRunning = emulators.Any(y => y == x.Id)
        });
        
        return Ok(result);
    }
    
    /// <summary>
    /// Get Reader by ID
    /// </summary>
    /// <param name="id">Id reader</param>
    /// <returns></returns>
    [HttpGet("GetReader/{id}")]
    [ProducesResponseType<Reader>(200)] // Ok
    public async Task<IActionResult> GetReader(Guid id)
    {
        var reader = await context.Readers.AsNoTracking()
            .Include(x => x.Config)
            .Include(x => x.Antennas)
            .FirstOrDefaultAsync(x => x.Id == id);

        return reader is null ? NoContent() : Ok(reader);
    }

    /// <summary>
    /// Create Reader
    /// </summary>
    /// <param name="readerDto"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("CreateReader")]
    public async Task<IActionResult> CreateReader(ReaderDto readerDto, CancellationToken token)
    {
        var reader = mapper.Map<Reader>(readerDto);
        await context.Readers.AddAsync(reader, token);
        await context.SaveChangesAsync(token);
        
        return Ok(reader);
    }

    [HttpPut("UpdateReader")]
    public async Task<IActionResult> UpdateReader(Guid id, Reader readerUpdate, CancellationToken token)
    {
        var reader = await context.Readers.AsNoTracking()
            .Include(reader => reader.Config)
            .Include(reader => reader.Antennas)
            .FirstOrDefaultAsync(x => x.Id == id, token);
        
        if(reader is null) return NotFound();
        
        readerUpdate.Config.Reader = reader;
        
        context.Readers.Update(readerUpdate);
        await context.SaveChangesAsync(token);

        await emulatorManager.Restart(readerUpdate, token);
        
        return Ok(readerUpdate);
    }

    [HttpPut("AddAntenna")]
    public async Task<IActionResult> AddAntenna(Guid idReader, Antenna antenna, CancellationToken token)
    {
        var reader = await context.Readers.AsNoTracking()
            .Include(reader => reader.Config)
            .Include(reader => reader.Antennas)
            .FirstOrDefaultAsync(x => x.Id == idReader, token);
        
        if(reader is null) return NotFound();
        
        reader.Antennas ??= new List<Antenna>();
        
        reader.Antennas.Add(antenna);
        await context.SaveChangesAsync(token);
        
        await emulatorManager.Restart(reader, token);

        return Ok(reader);
    }

    [HttpDelete("DeleteReader")]
    public async Task<IActionResult> DeleteReader(Guid id, CancellationToken token)
    {
       await emulatorManager.Stop(id, token);
       var reader = await context.Readers.AsNoTracking()
           .Include(x => x.Config)
           .Include(x => x.Antennas)
           .FirstOrDefaultAsync(x => x.Id == id, token);
       if(reader is null) return NotFound();
       
       context.Readers.Remove(reader);
       await context.SaveChangesAsync(token);
       return Ok();
    }
}