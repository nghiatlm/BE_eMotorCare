using BE_eMotoCare.API.Realtime.Hubs;
using eMototCare.BLL.Services.BackgroundServices;
using eMototCare.BLL.Services.NotificationServices;
using Microsoft.AspNetCore.SignalR;

namespace BE_eMotoCare.API.Realtime.Services
{
    public class NotificationCampaignService : INotifierCampaignService
    {
        private readonly IHubContext<NotificationCampaignHub> _hubContext;

        public NotificationCampaignService(IHubContext<NotificationCampaignHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyComingSoonAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCampaignComingSoon", entity, data);
        }

        public async Task NotifyExpiredAsync(string entity, object data)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCampaignExpired", entity, data);
        }
    }
}
