using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ModelRepository
{
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        public ModelRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Model?> GetByIdAsync(Guid id) =>
           await _context
                .Models
                .Include(x => x.MaintenancePlan)
                .FirstOrDefaultAsync(x => x.Id == id);
    }
}
