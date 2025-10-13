

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Domain.Entities
{
    [Index(nameof(CampaignName), IsUnique = true)]
    public class Campaign : BaseEntity
    {
        public Guid CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<CampaignDetail>? CampaignDetails { get; set; }
    }
}