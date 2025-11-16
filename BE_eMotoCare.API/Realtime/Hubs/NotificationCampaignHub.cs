using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BE_eMotoCare.API.Realtime.Hubs
{
    [AllowAnonymous]
    public class NotificationCampaignHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Campaign client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Campaign client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
