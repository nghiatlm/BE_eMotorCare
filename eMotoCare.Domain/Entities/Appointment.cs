

using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid AppointmentId { get; set; }
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
        public string CheckinQR { get; set; }

        public Customer? Customer { get; set; }
        public VehicleStage? VehicleStage { get; set; }
        public Branch? Branch { get; set; }
        public Staff? ApproveBy { get; set; }
        public Campaign? Campaign { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}