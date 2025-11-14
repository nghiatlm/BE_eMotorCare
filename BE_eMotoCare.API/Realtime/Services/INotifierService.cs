namespace BE_eMotoCare.API.Realtime.Services
{
    public interface INotifierService
    {
        Task NotifyUpdateAsync(string entity, object data);
        Task NotifyDeleteAsync(string entity, object data);
    }
}
