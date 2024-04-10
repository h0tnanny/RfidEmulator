namespace RfidEmulator.Api.Configurations;

public sealed class KafkaConfig
{
    public string? BootstrapServers { get; set; }
    public string? Topic { get; set; }
}