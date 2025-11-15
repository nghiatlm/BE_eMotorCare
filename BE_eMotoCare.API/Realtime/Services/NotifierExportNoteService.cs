using BE_eMotoCare.API.Realtime.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BE_eMotoCare.API.Realtime.Services
{

    
    public class NotifierExportNoteService : INotifierExportNoteService
    {
        private readonly IHubContext<NotificationExportNoteHub> _hubContext;
        public NotifierExportNoteService(IHubContext<NotificationExportNoteHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyCreateAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCreate", entity, data);
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
