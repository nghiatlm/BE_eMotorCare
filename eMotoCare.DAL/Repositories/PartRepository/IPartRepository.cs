using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PartRepository
{
    public interface IPartRepository : IGenericRepository<Part>
    {
        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsNameAsync(string name);
        Task<Part?> GetByIdAsync(Guid id);
        Task<List<Part>> FindPartTypeAsync(Guid partTypeId);
        Task<List<Part>> FindPartsByModelandType(Guid modelId, Guid partTypeId);
        Task<(IReadOnlyList<Part> Items, long Total)> GetPagedAsync(Guid? partTypeId, string? code, string? name, Status? status, int? quantity, Guid? serviceCenterId, int page, int pageSize);
        Task<List<EVCheckReplacementResponse>> GetReplacementPartsAsync(Guid modelId, Guid serviceCenterId);
    }
}
