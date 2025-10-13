

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class MaintenaceStage : BaseEntity
    {
        public Guid MaintenaceStageId { get; set; }
        public Guid MaintenancePlanId { get; set; }
        public MaintenancePlan? MaintenancePlan { get; set; }
        public string StageName { get; set; }
        public string StageOrder { get; set; }
        public string Duration { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MaintenaceStageDetail>? MaintenaceStageDetails { get; set; }

    }
}