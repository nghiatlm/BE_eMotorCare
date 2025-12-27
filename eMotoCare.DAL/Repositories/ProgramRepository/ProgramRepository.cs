
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ProgramRepository
{
    public class ProgramRepository : GenericRepository<Program>, IProgramRepository
    {
        public ProgramRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<string> GenerateProgramCodeAsync(ProgramType type)
        {
            var year = DateTime.Now.Year;
            int count = await _context.programs
                .Where(p => p.ProgramType == type && p.StartDate.Year == year)
                .CountAsync() + 1;
            return $"{type}-{year}-{count:D2}";
        }

        public async Task<Program?> FindById(Guid id)
        {
            return await _context.programs
                .Include(p => p.ProgramDetails)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PageResult<Program>> FindParams(string? query, DateTime? startDate, DateTime? enđate, ProgramType? type, Status? status, Guid? modelId = null, Guid? partId = null, ActionType? actionType = null, int? manufactureYear = null, int pageCurrent = 1, int pageSize = 10)
        {
            var q = _context.programs
                .Include(p => p.ProgramDetails)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var qLower = query.Trim().ToLower();
                q = q.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(qLower)) ||
                    (p.Code != null && p.Code.ToLower().Contains(qLower)) ||
                    (p.Description != null && p.Description.ToLower().Contains(qLower))
                );
            }
            if (startDate.HasValue)
            {
                q = q.Where(p => p.StartDate >= startDate.Value);
            }
            if (enđate.HasValue)
            {
                q = q.Where(p => p.EndDate != null && p.EndDate <= enđate.Value);
            }
            if (type.HasValue)
            {
                q = q.Where(p => p.ProgramType == type.Value);
            }
            if (status.HasValue)
            {
                q = q.Where(p => p.Status == status.Value);
            }


            // Lọc theo modelId nếu có
            if (modelId.HasValue)
            {
                q = q.Where(p => p.ProgramDetails.Any(d => d.ModelId == modelId.Value));
            }
            // Lọc theo partId nếu có
            if (partId.HasValue)
            {
                q = q.Where(p => p.ProgramDetails.Any(d => d.PartId == partId.Value));
            }
            // Lọc theo actionType nếu có
            if (actionType.HasValue)
            {
                q = q.Where(p => p.ProgramDetails.Any(d => d.ActionType == actionType.Value));
            }
            // Lọc theo manufactureYear nếu có
            if (manufactureYear.HasValue)
            {
                q = q.Where(p => p.ProgramDetails.Any(d => d.ManufactureYear == manufactureYear.Value));
            }

            var total = await q.CountAsync();
            var items = await q
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageCurrent - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResult<Program>(items, total, pageCurrent, pageSize);
        }
    }
}