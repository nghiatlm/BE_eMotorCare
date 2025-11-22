using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using eMotoCare.DAL.Repositories.BatteryCheckRepository;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.BatteryCheckRepository
{
    public class BatteryCheckRepository : GenericRepository<BatteryCheck>, IBatteryCheckRepository
    {
        public BatteryCheckRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<BatteryCheck?> GetByEVCheckDetailIdAsync(Guid evCheckDetailId)
        {
            return _context
                .BatteryChecks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.EVCheckDetailId == evCheckDetailId);
        }
    }
}
