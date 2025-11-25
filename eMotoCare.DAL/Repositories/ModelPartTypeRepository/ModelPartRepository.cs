using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ModelPartTypeRepository
{
    public class ModelPartRepository : GenericRepository<ModelPart>, IModelPartRepository
    {
        public ModelPartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
