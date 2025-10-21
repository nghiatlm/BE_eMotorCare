using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace eMotoCare.DAL.Repositories.VehicleRepository
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id) =>
           await _context
                .Vehicles
                .Include(x => x.Model)
                .FirstOrDefaultAsync(x => x.Id == id);
    }
}
