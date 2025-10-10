
using eMotoCare.Domain.Common;

namespace eMotoCare.Domain.Entities
{
    public class CampaignDetail : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public string Description { get; set; }
        public Guid PartId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Part? Part { get; set; }

    }
}