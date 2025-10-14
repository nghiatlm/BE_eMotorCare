

using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Entities
{
    public class Part : BaseEntity
    {
        public Guid PartId { get; set; }
        public Guid PartTypeId { get; set; }
        public PartType? PartType { get; set; }
        public string PartName { get; set; }
        public string PartCode { get; set; }
        public string Manufacturer { get; set; }
        public int Quantity { get; set; }
        public string Origin { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CampaignDetail>? CampaignDetails { get; set; }
        public virtual ICollection<MaintenaceStageDetail>? MaintenaceStageDetails { get; set; }

    }
}