using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.BranchRepository;

namespace eMotoCare.DAL
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IBranchRepository Branches { get; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesWithTransactionAsync();
        int SaveChangesWithTransaction(); // Lưu thay đổi với transaction đồng bộ
    }
}
