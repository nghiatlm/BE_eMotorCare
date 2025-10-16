using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.VehiclePartItemRepository
{
    public class VehiclePartItemRepository : GenericRepository<VehiclePartItem>, IVehiclePartItemRepository
    {
        public VehiclePartItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
