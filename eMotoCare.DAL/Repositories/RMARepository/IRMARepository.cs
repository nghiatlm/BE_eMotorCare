using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.RMARepository
{
    public interface IRMARepository : IGenericRepository<RMA>
    {
        Task<bool> ExistsCodeAsync(string code);
        Task<List<RMA?>> GetByCustomerIdAsync(Guid customerId);
        Task<RMA?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<RMA> Items, long Total)> GetPagedAsync(string? code, DateTime? fromDate, DateTime? toDate, string? returnAddress, RMAStatus? status, Guid? createdById, Guid? serviceCenterId, int page, int pageSize);
    }
}
