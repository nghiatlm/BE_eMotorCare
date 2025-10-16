using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.StaffRepository
{
    public class StaffRepository : GenericRepository<ServiceCenter>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
