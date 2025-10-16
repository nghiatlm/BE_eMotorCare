using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ModelRepository
{
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        public ModelRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
