namespace RfidEmulator.Api.Services;

public interface IOptimizationService
{
    public Task Start(CancellationToken cancellationToken);
    public Task Stop(CancellationToken cancellationToken);
}