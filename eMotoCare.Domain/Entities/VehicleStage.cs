
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class VehicleStage : BaseEntity
    {
        public Guid VehicleStageId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid MaintenaceStageDetailId { get; set; }
        public DateTime DateOfImplementation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }

        public Vehicle? Vehicle { get; set; }
        public MaintenaceStageDetail? MaintenanceStageDetail { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }

    }
}