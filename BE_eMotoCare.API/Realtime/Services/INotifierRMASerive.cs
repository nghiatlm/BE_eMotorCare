namespace BE_eMotoCare.API.Realtime.Services
{
    public interface INotifierRMASerive
    {
        Task NotifyUpdateAsync(string entity, object data);
        Task NotifyDeleteAsync(string entity, object data);
    }
}
