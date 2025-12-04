using BE_eMotoCare.API.Realtime.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BE_eMotoCare.API.Realtime.Services
{
    public class NotifierRMASerive : INotifierRMASerive
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotifierRMASerive(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUpdateAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", entity, data);
        }

        public async Task NotifyCreateAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCreate", entity, data);
        }

        public async Task NotifyDeleteAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDelete", entity, data);
        }
    }
}
