


using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.PartTypeServices
{
    public interface IPartTypeService
    {
        Task<PageResult<PartTypeResponse>> GetPagedAsync(string? name, string? description, int page = 1, int pageSize = 10);
    }
}
