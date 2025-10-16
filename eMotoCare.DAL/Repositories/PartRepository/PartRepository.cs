using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.PartRepository
{
    public class PartRepository : GenericRepository<Part>, IPartRepository
    {
        public PartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
