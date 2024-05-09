using System.Runtime.InteropServices.ComTypes;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RfidEmulator.Api.Configurations;

namespace RfidEmulator.Api.Services;

public class KafkaProducer : IKafkaProducer
{ 
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly KafkaConfig _config;
    private readonly IHubContext<EmulatorHub> _hub;
    
    public KafkaProducer(IOptions<KafkaConfig> options, ILogger<KafkaProducer> logger, IHubContext<EmulatorHub> hub)
    {
        _logger = logger;
        _config = options.Value;
        var config = new ProducerConfig { BootstrapServers = options.Value.BootstrapServers};
        _producer = new ProducerBuilder<string, string>(config).Build();
        _hub = hub;
    }

    public async Task ProduceAsync(string message, CancellationToken cancellationToken)
    {
        try
        {
            await _producer.ProduceAsync(_config.Topic, new()
            {
                Value = message
            }, cancellationToken);
            
            await _hub.Clients.All.SendAsync("Receive", message, cancellationToken: cancellationToken);
        }
        catch (ProduceException<string, string> e)
        {
            await _hub.Clients.All.SendAsync("Receive",$"Delivery failed: {e.Error.Reason}", cancellationToken: cancellationToken);
        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
