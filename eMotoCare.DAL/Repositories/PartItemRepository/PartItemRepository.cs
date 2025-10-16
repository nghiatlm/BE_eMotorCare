using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.PartItemRepository
{
    public class PartItemRepository : GenericRepository<PartItem>, IPartItemRepository
    {
        public PartItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
