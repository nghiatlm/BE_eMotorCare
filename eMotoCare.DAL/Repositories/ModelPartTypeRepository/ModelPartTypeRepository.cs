using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ModelPartTypeRepository
{
    public class ModelPartTypeRepository : GenericRepository<ModelPartType>, IModelPartTypeRepository
    {
        public ModelPartTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
