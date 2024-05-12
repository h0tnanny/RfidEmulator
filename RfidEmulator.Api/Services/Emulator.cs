using Newtonsoft.Json;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Services;

public class Emulator(Reader reader, IKafkaProducer producer) : BackgroundService, IEmulator
{
    private CancellationTokenSource CancellationToken { get; set; } = new();

    public bool IsRunning { get; private set; }
    
    public void Start()
    {
        CancellationToken = new();
        IsRunning = true;

        ExecuteAsync(CancellationToken.Token);
    }

    public Task Stop()
    {
        CancellationToken.Cancel();
        IsRunning = false;

        return Task.CompletedTask;
    }

    private async Task Emulation(CancellationToken stoppingToken)
    {
        var config = reader.Config;
    
        var tagsId = Enumerable.Range(0, config.Tags)
            .Select(x => Guid.NewGuid()).ToList();
        var antennasId = reader.Antennas?.Select(x => Guid.NewGuid()).ToList() ?? throw new ArgumentNullException(nameof(reader.Antennas));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = Enumerable.Range(config.CountsPerSecTimeMin, config.CountsPerSecTimeMax)
                .Select(x => new
                {
                    Reader = JsonConvert.SerializeObject(config, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    TagsId = tagsId[new Random().Next(0, tagsId.Count)],
                    AntennaId = antennasId[new Random().Next(0, antennasId.Count)],
                    Rssi = new Random().Next(config.UpperRssiLevelMin, config.UpperRssiLevelMax),
                    CountPerSec = new Random().Next(config.CountsPerSecTimeMin, config.CountsPerSecTimeMax),
                    DateTime = DateTime.UtcNow
                });

            foreach (var message in messages)
            {
                var json = JsonConvert.SerializeObject(message);
                await producer.ProduceAsync(json, stoppingToken);
                await Task.Delay(new Random().Next(0, 10), stoppingToken);
            }
        
            await Task.Delay(200, stoppingToken);
        }
    }


    public override void Dispose()
    {
        Stop();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Emulation(stoppingToken);
    }
}