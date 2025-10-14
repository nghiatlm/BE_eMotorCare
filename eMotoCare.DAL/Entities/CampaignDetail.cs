
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Entities
{
    public class CampaignDetail : BaseEntity
    {
        public Guid CampaignDetailId { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
        public string Description { get; set; }
        public Guid PartId { get; set; }
        public Part? Part { get; set; }
        public Guid EVCheckDetailId { get; set; }
        public EVCheckDetail? EVCheckDetail { get; set; }

    }
}