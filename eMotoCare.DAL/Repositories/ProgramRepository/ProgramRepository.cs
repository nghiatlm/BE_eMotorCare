
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

        public async Task<bool> ExistsTitleAsync(string title)
        {
            return await _context.programs.AnyAsync(x => x.Title == title);
        }

        public async Task<Program?> FindById(Guid id)
        {
            return await _context.programs
                .Include(p => p.ProgramDetails)
                .Include(p => p.ProgramModels)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PageResult<Program>> FindParams(string? query, DateTime? startDate, DateTime? enđate, ProgramType? type, Status? status, Guid? modelId, int pageCurrent = 1, int pageSize = 10)
        {
            var q = _context.programs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var qLower = query.Trim().ToLower();
                q = q.Where(p => p.Title.ToLower().Contains(qLower) || (p.Description != null && p.Description.ToLower().Contains(qLower)));
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
                q = q.Where(p => p.Type == type.Value);
            }
            if (status.HasValue)
            {
                q = q.Where(p => p.Status == status.Value);
            }
            if (modelId.HasValue)
            {
                q = q.Where(p => p.ProgramModels != null && p.ProgramModels.Any(pm => pm.VehicleModelId == modelId.Value));
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