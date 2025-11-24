
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ProgramService
{
    public interface IProgramService
    {
        Task<bool> Create(ProgramRequest request);
        Task<ProgramDetailResponse?> GetById(Guid id);
        Task<PageResult<ProgramResponse>> GetPaged(string? query, DateTime? startDate, DateTime? endDate, ProgramType? type, Status? status, Guid? modelId, int pageCurrent = 1, int pageSize = 10);
    }
}