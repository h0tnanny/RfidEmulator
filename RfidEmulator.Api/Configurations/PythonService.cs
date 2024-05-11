namespace RfidEmulator.Api.Configurations;

public class PythonService
{
    public string KafkaGroupId { get; set; }
    public string? KafkaTopic { get; set; }
    public string HostServer { get; set; }
    public string EndpointName { get; set; }
}