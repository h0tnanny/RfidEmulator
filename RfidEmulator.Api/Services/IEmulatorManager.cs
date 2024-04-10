using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Services;

public interface IEmulatorManager : IDisposable
{
    Task Stop(Guid id, CancellationToken token);
    Task Start(Reader reader, CancellationToken token);
    Task Restart(Reader reader, CancellationToken token);
    public IEnumerable<Guid> GetEmulators();
}