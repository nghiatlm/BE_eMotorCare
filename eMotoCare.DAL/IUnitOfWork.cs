
namespace eMotoCare.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();
    }
}