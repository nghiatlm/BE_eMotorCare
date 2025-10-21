

using eMotoCare.BO.DTO.Requests;

namespace eMototCare.BLL.Services.EVCheckServices
{
    public interface IEVCheckService
    {
        Task<Guid> CreateAsync(EVCheckRequest req);
    }
}
