using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.BranchRepository;
using eMotoCare.DAL.Repositories.ServiceCenterRepository;
using eMotoCare.DAL.Repositories.StaffRepository;

namespace eMotoCare.DAL
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IBranchRepository Branches { get; }
        IServiceCenterRepository ServiceCenters { get; }
        IStaffRepository Staffs { get; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesWithTransactionAsync();
        int SaveChangesWithTransaction(); // Lưu thay đổi với transaction đồng bộ
    }
}
