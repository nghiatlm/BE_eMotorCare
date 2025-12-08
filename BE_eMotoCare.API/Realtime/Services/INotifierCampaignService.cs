namespace eMototCare.BLL.Services.NotificationServices
{
    public interface INotifierCampaignService
    {
        Task NotifyComingSoonAsync(string entity, object data);
        Task NotifyExpiredAsync(string entity, object data);
    }
}
