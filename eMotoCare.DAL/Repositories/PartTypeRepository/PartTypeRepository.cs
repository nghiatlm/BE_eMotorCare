using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.PartTypeRepository
{
    public class PartTypeRepository : GenericRepository<PartType>, IPartTypeRepository
    {
        public PartTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
