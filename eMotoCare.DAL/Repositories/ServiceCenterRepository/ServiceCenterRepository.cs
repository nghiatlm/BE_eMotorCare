using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ServiceCenterRepository
{
    public class ServiceCenterRepository : GenericRepository<ServiceCenter>, IServiceCenterRepository
    {
        public ServiceCenterRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
