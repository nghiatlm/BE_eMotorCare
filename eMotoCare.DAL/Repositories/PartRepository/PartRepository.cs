using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.PartRepository
{
    public class PartRepository : GenericRepository<Part>, IPartRepository
    {
        public PartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<Part> Items, long Total)> GetPagedAsync(
            Guid? partTypeId,
            string? code,
            string? name,
            Status? status,
            int? quantity,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Parts
                .Include(x => x.PartType)
                .AsNoTracking()
                .AsQueryable();
            if (partTypeId.HasValue)
                q = q.Where(x => x.PartTypeId == partTypeId.Value);

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(x => x.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(code))
                q = q.Where(x => x.Code.Contains(code));

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            if (quantity.HasValue)
                q = q.Where(x => x.Quantity == quantity.Value);



            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<Part?> GetByIdAsync(Guid id) =>
        _context.Parts.FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Parts.AnyAsync(x => x.Code == code);

        public async Task<bool> ExistsNameAsync(string name) =>
          await _context.Parts.AnyAsync(x => x.Name == name);

        public async Task<List<Part>> FindPartTypeAsync(Guid partTypeId) => await _context.Parts
            .Where(x => x.PartTypeId == partTypeId && x.Status == Status.ACTIVE)
            .ToListAsync();

        public async Task<List<EVCheckReplacementResponse>> GetReplacementPartsAsync(Guid modelId, Guid serviceCenterId)
        {
            var result = await _context.ModelParts
                            .Where(mp => mp.ModelId == modelId)


                            .Select(mp => mp.Part)


                            .Select(p => new EVCheckReplacementResponse
                            {
                                PartId = p.Id,
                                PartName = p.Name,
                                Code = p.Code,
                                StockQuantity = p.PartItems
                                .Count(pi => pi.ServiceCenterInventory.ServiceCenterId == serviceCenterId)
                            })
                            .ToListAsync();

            return result;

        }

        public async Task<List<Part>> FindPartsByModelandType(Guid modelId, Guid partTypeId)
        {
            return await _context.Parts
                .Where(p => p.PartTypeId == partTypeId)
                .Where(p => p.ModelParts.Any(mp => mp.ModelId == modelId))
                .ToListAsync();
        }
    }
}
