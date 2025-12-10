using Microsoft.AspNetCore.SignalR;
using BE_eMotoCare.API.Realtime.Hubs;


namespace BE_eMotoCare.API.Realtime.Services
{
    

    public class NotifierService : INotifierService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotifierService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUpdateAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", entity, data);
        }

        public async Task NotifyDeleteAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDelete", entity, data);
        }

        public async Task NotifyCreateAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCreate", entity, data);
        }
    }
}
