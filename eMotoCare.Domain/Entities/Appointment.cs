

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VehicleStageId { get; set; }
        public Guid BranchId { get; set; }
        public Guid ApproveById { get; set; }
        public Guid CampaignId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string FullName { get; set; }
        public Status Status { get; set; }
        public ServiceType ServiceType { get; set; }
        public string? Note { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual VehicleStage? VehicleStage { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual Staff? ApproveBy { get; set; }
        public virtual Campaign? Campaign { get; set; }
    }
}