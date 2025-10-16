using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ServiceCenterInventoryRepository
{
    public class ServiceCenterInventoryRepository : GenericRepository<ServiceCenterInventory>, IServiceCenterInventoryRepository
    {
        public ServiceCenterInventoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
