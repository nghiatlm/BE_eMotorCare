

using eMotoCare.Application.Interfaces.IRepository;

namespace eMotoCare.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesWithTransactionAsync();
        int SaveChangesWithTransaction(); // Lưu thay đổi với transaction đồng bộ
    }
}
