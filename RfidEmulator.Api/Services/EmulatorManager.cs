using System.Collections.Concurrent;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Services;

public sealed class EmulatorManager(ILogger<EmulatorManager> logger, IKafkaProducer producer) : IEmulatorManager
{
    private readonly ConcurrentDictionary<Guid, IEmulator> _emulators = new();
    
    public Task Start(Reader reader, CancellationToken token)
    {
        var emulator = new Emulator(reader, producer);
        if (_emulators.TryAdd(reader.Id, emulator))
        {
            emulator.Start();
        }

        return Task.CompletedTask;
    }

    public async Task Stop(Guid id, CancellationToken token)
    {
        if (_emulators.TryRemove(id, out var emulator))
        {
            await emulator.Stop();
        }
    }
    
    public async Task Restart(Reader reader, CancellationToken token)
    {
        if(_emulators.TryGetValue(reader.Id, out var emulator))
        {
            await Stop(reader.Id, token);
            await Start(reader, token);
        }
    }

    public IEnumerable<Guid> GetEmulators()
    {
        return _emulators.Keys;
    }

    public void Dispose()
    {
        foreach (var item in _emulators)
        {
            item.Value.Dispose();
        }
        
        _emulators.Clear();
    }
}