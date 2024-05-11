using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RfidEmulator.Api.Configurations;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Services;

public sealed class OptimizationService : BackgroundService, IOptimizationService
{
    private readonly IOptions<PythonService> _optionsService;
    private readonly IOptions<KafkaConfig> _optionsKafka;
    private readonly IServiceProvider _serviceProvider;

    public OptimizationService(IOptions<PythonService> optionsService, 
        IOptions<KafkaConfig> optionsKafka, IServiceProvider serviceProvider)
    {
        _optionsService = optionsService;
        _optionsKafka = optionsKafka;
        _serviceProvider = serviceProvider;
    }
    
    private bool IsEnabled { get; set; }
    private CancellationTokenSource StopToken { get; set; }

    private async Task StartConsumerLoop(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = _optionsService.Value.KafkaGroupId,
            BootstrapServers = _optionsKafka.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        var topic = _optionsService.Value.KafkaTopic;

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        using var scope = _serviceProvider.CreateScope();
        var readerServices = scope.ServiceProvider.GetRequiredService<IReaderService>();
        consumer.Subscribe(topic);
        IsEnabled = true;
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = await Task.Run(() => consumer.Consume(100), stoppingToken);
                    if (consumeResult is null)
                    {
                        scope.Dispose();
                        continue;
                    }
                    var message = consumeResult.Message.Value;

                    var optimization = JsonConvert.DeserializeObject<OptimizationConfig>(message);

                    if (optimization == null) continue;
                    
                    var reader = readerServices.Get(optimization.ReaderId, stoppingToken).Result;
                    if (reader == null) continue;
                        
                    if (optimization.CountsPerSecTimeMin.HasValue)
                        reader.Config.CountsPerSecTimeMin = optimization.CountsPerSecTimeMin.Value;
                            
                    if (optimization.CountsPerSecTimeMax.HasValue)
                        reader.Config.CountsPerSecTimeMax = optimization.CountsPerSecTimeMax.Value;
                    
                    if (optimization.UpperRssiLevelMin.HasValue)
                        reader.Config.UpperRssiLevelMin = optimization.UpperRssiLevelMin.Value;
                    
                    if (optimization.UpperRssiLevelMax.HasValue)
                        reader.Config.UpperRssiLevelMax = optimization.UpperRssiLevelMax.Value;
                    
                    if (optimization.Tags.HasValue)
                        reader.Config.Tags = optimization.Tags.Value;
                    
                    await readerServices.Update(optimization.ReaderId, reader, stoppingToken);
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return StartConsumerLoop(stoppingToken);
    }

    public Task Start(CancellationToken cancellationToken)
    {
        if (IsEnabled) return Task.CompletedTask;
        
        StopToken = new();
        
        return ExecuteAsync(StopToken.Token);
    }

    public Task Stop(CancellationToken cancellationToken)
    {
        StopToken.Cancel();
        IsEnabled = false;
        
        return Task.CompletedTask;
    }
}