
using eMotoCare.Common.Enums;


namespace eMotoCare.DAL.Entities
{
    public class MaintenancePlan : BaseEntity
    {
        public Guid MaintenancePlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanCode { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public int TotalStage { get; set; }
        public IntervalType IntervalType { get; set; }
        public IntervalUnit IntervalUnit { get; set; }

        public virtual ICollection<MaintenaceStage>? MaintenaceStages { get; set; }
        public virtual ICollection<Model>? Models { get; set; }

    }
}