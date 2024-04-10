using Microsoft.AspNetCore.SignalR;

namespace RfidEmulator.Api.Services;

public class EmulatorHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("Receive", message);
    }
}