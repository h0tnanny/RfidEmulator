using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RfidEmulator.Domain.DTOs;
using RfidEmulator.Domain.Entity;
using RfidEmulator.Repository;

namespace RfidEmulator.Api.Services;

public class ReaderService(RepositoryContext context, IMapper mapper,
    IEmulatorManager emulatorManager) : IReaderService
{
    public async Task<Reader?> Get(Guid id, CancellationToken token)
    {
        var reader = await context.Readers.AsNoTracking()
            .Include(x => x.Config)
            .Include(x => x.Antennas)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
        
        return reader;
    }

    public async Task<ICollection<Reader>?> GetReaders()
    {
        var readers = await context.Readers.AsNoTracking()
            .Include(x => x.Config)
            .Include(x => x.Antennas)
            .ToListAsync();

        return readers;
    }

    public async Task<Reader> Create(ReaderDto readerDto, CancellationToken token)
    {
        var reader = mapper.Map<Reader>(readerDto);
        await context.Readers.AddAsync(reader, token);
        await context.SaveChangesAsync(token);
        
        return reader;
    }

    public async Task<Reader?> Update(Guid id, Reader readerUpdate, CancellationToken token)
    {
        var reader = await context.Readers.AsNoTracking()
            .Include(reader => reader.Config)
            .Include(reader => reader.Antennas)
            .FirstOrDefaultAsync(x => x.Id == id, token);

        if (reader is null) return null;
        
        readerUpdate.Config.Reader = reader;
        
        context.Readers.Update(readerUpdate);
        await context.SaveChangesAsync(token);

        await emulatorManager.Restart(readerUpdate, token);

        return reader;
    }

    public async Task Remove(Guid id, CancellationToken token)
    {
        await emulatorManager.Stop(id, token);
        var reader = await context.Readers.AsNoTracking()
            .Include(x => x.Config)
            .Include(x => x.Antennas)
            .FirstOrDefaultAsync(x => x.Id == id, token);
        
        if(reader is null) return;
       
        context.Readers.Remove(reader);
        await context.SaveChangesAsync(token);
    }

    public async Task<Reader?> AddAntenna(Guid idReader, AntennaDto antennaDto, CancellationToken token)
    {
        var antenna = mapper.Map<Antenna>(antennaDto);
        
        var reader = await context.Readers.AsNoTracking()
            .Include(reader => reader.Config)
            .Include(reader => reader.Antennas)
            .FirstOrDefaultAsync(x => x.Id == idReader, token);
        
        if(reader is null) return null;
        
        reader.Antennas ??= new List<Antenna>();
        reader.Antennas.Add(antenna);
        
        context.Antennas.Add(antenna);
        context.Readers.Update(reader);
        
        await context.SaveChangesAsync(token);
        
        await emulatorManager.Restart(reader, token);

        return reader;
    }
}