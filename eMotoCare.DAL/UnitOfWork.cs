using eMotoCare.DAL.Context;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMotoCare.DAL.Repositories.BranchRepository;
using eMotoCare.DAL.Repositories.ServiceCenterRepository;
using eMotoCare.DAL.Repositories.StaffRepository;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContextMotoCare _unitOfWorkContext;
        private IAccountRepository _accountRepository;
        private IBranchRepository _branchRepository;
        private IServiceCenterRepository _serviceCenterRepository;
        private IStaffRepository _staffRepository;

        public UnitOfWork(DBContextMotoCare context)
        {
            _unitOfWorkContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAccountRepository Accounts =>
            _accountRepository ??= new AccountRepository(_unitOfWorkContext);
        public IStaffRepository Staffs =>
            _staffRepository ??= new StaffRepository(_unitOfWorkContext);

        public IBranchRepository Branches =>
            _branchRepository ??= new BranchRepository(_unitOfWorkContext);
        public IServiceCenterRepository ServiceCenters =>
            _serviceCenterRepository ??= new ServiceCenterRepository(_unitOfWorkContext);

        // SaveChangesWithTransaction đồng bộ
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _unitOfWorkContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"SaveChanges failed: {ex.Message}", ex);
            }
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            var strategy = _unitOfWorkContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                // Tạo một transaction scope
                await using var transaction =
                    await _unitOfWorkContext.Database.BeginTransactionAsync();
                try
                {
                    // Thực hiện lưu thay đổi
                    var result = await _unitOfWorkContext.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Transaction failed: {ex.Message}", ex);
                }
            });
        }

        public int SaveChangesWithTransaction()
        {
            var strategy = _unitOfWorkContext.Database.CreateExecutionStrategy();
            return strategy.Execute(() =>
            {
                using var transaction = _unitOfWorkContext.Database.BeginTransaction();
                try
                {
                    var result = _unitOfWorkContext.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException($"Transaction failed: {ex.Message}", ex);
                }
            });
        }
    }
}
