


using eMotoCare.DAL.Repositories.AccountRepository;

namespace eMotoCare.DAL
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesWithTransactionAsync();
        int SaveChangesWithTransaction(); // Lưu thay đổi với transaction đồng bộ
    }
}
