using System.Runtime.InteropServices.ComTypes;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using RfidEmulator.Api.Configurations;

namespace RfidEmulator.Api.Services;

public class KafkaProducer : IKafkaProducer
{ 
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly KafkaConfig _config;
    private readonly EmulatorHub _hub;
    
    public KafkaProducer(IOptions<KafkaConfig> options, ILogger<KafkaProducer> logger, EmulatorHub hub)
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
            var deliveryReport = await _producer.ProduceAsync(_config.Topic, new() { Value = message }, cancellationToken);
            Console.WriteLine($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'");
            await _hub.SendMessage(message);
        }
        catch (ProduceException<string, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            await _hub.SendMessage($"Delivery failed: {e.Error.Reason}");
        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
