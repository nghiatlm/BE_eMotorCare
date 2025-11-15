namespace BE_eMotoCare.API.Realtime.Services
{
    public interface INotifierExportNoteService
    {
        Task NotifyCreateAsync(string entity, object data);
        Task NotifyUpdateAsync(string entity, object data);
        Task NotifyDeleteAsync(string entity, object data);
    }
}
