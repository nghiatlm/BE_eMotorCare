using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.BranchRepository;
using eMotoCare.DAL.Repositories.ServiceCenterRepository;

namespace eMotoCare.DAL
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IBranchRepository Branches { get; }
        IServiceCenterRepository ServiceCenters { get; }
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesWithTransactionAsync();
        int SaveChangesWithTransaction(); // Lưu thay đổi với transaction đồng bộ
    }
}
