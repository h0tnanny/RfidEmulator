using RfidEmulator.Domain.DTOs;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Services;

public interface IReaderService
{
    public Task<Reader?> Get(Guid id, CancellationToken token = default);
    public Task<ICollection<Reader>?> GetReaders();
    public Task<Reader> Create(ReaderDto readerDto, CancellationToken token = default);
    public Task<Reader?> Update(Guid id, Reader readerUpdate, CancellationToken token = default);
    public Task Remove(Guid id, CancellationToken token = default);
    public Task<Reader?> AddAntenna(Guid idReader, AntennaDto antennaDto, CancellationToken token = default);
    public Task RemoveAntenna(Guid id, CancellationToken token = default);
}