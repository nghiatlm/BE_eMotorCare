using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ServiceCenterRepository
{
    public interface IServiceCenterRepository : IGenericRepository<ServiceCenter>
    {
        Task<(IReadOnlyList<ServiceCenter> Items, long Total)> GetPagedAsync(
            string? search,
            StatusEnum? status,
            int page,
            int pageSize
        );

        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsEmailAsync(string email);
        Task<bool> ExistsPhoneAsync(string phone);
        Task<ServiceCenter?> GetByCodeAsync(string code);
        Task<ServiceCenter?> GetByIdAsync(Guid id);
    }
}
