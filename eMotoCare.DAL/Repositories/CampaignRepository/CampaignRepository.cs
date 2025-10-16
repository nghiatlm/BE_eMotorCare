using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.CampaignRepository
{
    public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
