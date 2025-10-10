
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class VehicleStage : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid MaintenaceStageDetailId { get; set; }
        public DateTime DateOfImplementation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }

        public virtual Vehicle? Vehicle { get; set; }
        public virtual MaintenaceStageDetail? MaintenanceStageDetail { get; set; }

    }
}