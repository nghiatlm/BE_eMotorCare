
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ProgramRepository
{
    public interface IProgramRepository : IGenericRepository<Program>
    {
        Task<Program?> FindById(Guid id);
        Task<string> GenerateProgramCodeAsync(ProgramType type);
        Task<PageResult<Program>> FindParams(string? query, DateTime? startDate, DateTime? enđate, ProgramType? type, Status? status, Guid? modelId = null, Guid? partId = null, ActionType? actionType = null, int? manufactureYear = null, int pageCurrent = 1, int pageSize = 10);
    }
}