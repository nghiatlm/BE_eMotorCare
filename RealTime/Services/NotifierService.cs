using Microsoft.AspNetCore.SignalR;
using RealTime.Hubs;


namespace RealTime.Services
{
    public interface INotifierService
    {
        Task NotifyUpdateAsync(string entity, object data);
        Task NotifyDeleteAsync(string entity, object data);
    }

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
    }
}
