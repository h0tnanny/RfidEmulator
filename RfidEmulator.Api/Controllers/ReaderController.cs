using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using RfidEmulator.Api.Services;
using RfidEmulator.Domain.DTOs;
using RfidEmulator.Domain.Entity;
using RfidEmulator.Repository;

namespace RfidEmulator.Api.Controllers;

public class ReaderController(IReaderService readerService, ILogger<ReaderController> logger, RepositoryContext context, IMapper mapper,
    IEmulatorManager emulatorManager) :
    BaseController<ReaderController>(logger, context, mapper)
{
    [HttpGet("GetReaders")]
    [ProducesResponseType<IEnumerable<Reader>>(200)]
    public async Task<IActionResult> GetReaders()
    {
        var readers = await readerService.GetReaders();

        if (readers is null) return NoContent();
        
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("GetReader/{id}")]
    [ProducesResponseType<Reader>(200)] // Ok
    public async Task<IActionResult> GetReader(Guid id, CancellationToken cancellationToken)
    {
        var reader = await readerService.Get(id, cancellationToken);

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
        var reader = await readerService.Create(readerDto, token);
        
        return Ok(reader);
    }

    /// <summary>
    /// Update a reader
    /// </summary>
    /// <param name="id"></param>
    /// <param name="readerUpdate"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut("UpdateReader")]
    public async Task<IActionResult> UpdateReader(Guid id, Reader readerUpdate, CancellationToken token)
    {
        var reader = await readerService.Update(id, readerUpdate, token);
        
        return Ok(reader);
    }

    /// <summary>
    /// Add antenna to reader by id
    /// </summary>
    /// <param name="idReader"></param>
    /// <param name="antennaDto"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut("AddAntenna")]
    public async Task<IActionResult> AddAntenna(Guid idReader, AntennaDto antennaDto, CancellationToken token)
    {
        var reader = await readerService.AddAntenna(idReader, antennaDto, token);

        return Ok(reader);
    }

    /// <summary>
    /// Delete a reader
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete("DeleteReader")]
    public async Task<IActionResult> DeleteReader(Guid id, CancellationToken token)
    {
        await readerService.Remove(id, token);
        
       return NoContent();
    }

    /// <summary>
    /// Remove a antenna by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete("RemoveAntenna")]
    public async Task<IActionResult> RemoveAntenna(Guid id, CancellationToken token = default)
    {
        await readerService.RemoveAntenna(id, token);

        return NoContent();
    }
}