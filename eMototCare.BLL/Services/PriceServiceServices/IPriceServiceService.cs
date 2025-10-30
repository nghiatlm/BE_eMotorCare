using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.PriceServiceServices
{
    public interface IPriceServiceService
    {
        Task<PageResult<PriceServiceResponse>> GetPagedAsync(
            string? search,
            Guid? partTypeId,
            Remedies? remedies,
            DateTime? fromEffectiveDate,
            DateTime? toEffectiveDate,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize
        );

        Task<PriceServiceResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(PriceServiceRequest req);
        Task UpdateAsync(Guid id, PriceServiceRequest req);
        Task DeleteAsync(Guid id);
    }
}
