namespace RfidEmulator.Api.Services;

public interface IKafkaProducer : IDisposable
{
    Task  ProduceAsync(string message, CancellationToken cancellationToken);
}
