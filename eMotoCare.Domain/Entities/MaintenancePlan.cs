
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class MaintenancePlan : BaseEntity
    {
        public Guid Id { get; set; }
        public string PlanName { get; set; }
        public string PlanCode { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public int TotalStage { get; set; }
        public IntervalType IntervalType { get; set; }
        public IntervalUnit IntervalUnit { get; set; }

    }
}