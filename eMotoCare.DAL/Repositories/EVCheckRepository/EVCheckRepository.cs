using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.EVCheckRepository
{
    public class EVCheckRepository : GenericRepository<EVCheck>, IEVCheckRepository
    {
        public EVCheckRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<EVCheck?> GetByAppointmentIdAsync(Guid appointmentId) =>
            _context
                .EVChecks.Include(x => x.EVCheckDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);

        public Task<EVCheck?> GetByIdIncludeDetailsAsync(Guid evCheckId) =>
            _context
                .EVChecks.Include(x => x.EVCheckDetails)
                .FirstOrDefaultAsync(x => x.Id == evCheckId);
    }
}
