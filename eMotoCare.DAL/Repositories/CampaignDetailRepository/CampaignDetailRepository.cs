using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.CampaignDetailRepository
{
    public class CampaignDetailRepository : GenericRepository<CampaignDetail>, ICampaignDetailRepository
    {
        public CampaignDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
