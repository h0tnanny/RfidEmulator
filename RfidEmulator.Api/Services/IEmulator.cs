namespace RfidEmulator.Api.Services;

public interface IEmulator : IDisposable
{
    bool IsRunning { get; }
    Task Stop();
}