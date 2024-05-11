using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RfidEmulator.Api.Configurations;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.Services;

public sealed class OptimizationService(IOptions<PythonService> optionsService, 
    IOptions<KafkaConfig> optionsKafka, IReaderService readerService) 
    : BackgroundService, IOptimizationService
{
    private bool IsEnabled { get; set; }
    private CancellationToken StopToken { get; set; }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = optionsService.Value.KafkaGroupId,
            BootstrapServers = optionsKafka.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        var topic = optionsService.Value.KafkaTopic;

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);
        try
        {
            while (!StopToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(CancellationToken.None);
                    var message = consumeResult.Message.Value;

                    var optimization = JsonConvert.DeserializeObject<OptimizationConfig>(message);

                    if (optimization == null) continue;
                    
                    var reader = readerService.Get(optimization.ReaderId, stoppingToken).Result;
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

                    readerService.Update(optimization.ReaderId, reader, stoppingToken);
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
        
        return Task.CompletedTask;
    }

    public Task Start(CancellationToken cancellationToken)
    {
        if (IsEnabled) return Task.CompletedTask;
        
        StopToken = new();

        ExecuteAsync(cancellationToken);

        IsEnabled = true;

        return Task.CompletedTask;
    }

    public Task Stop(CancellationToken cancellationToken)
    {
        StopToken.ThrowIfCancellationRequested();
        IsEnabled = false;
        
        return Task.CompletedTask;
    }
}