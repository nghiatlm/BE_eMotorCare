
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class EVCheck : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid TaskExecutorId { get; set; }
        public DateTime CheckDate { get; set; }
        public Status Status { get; set; }
        public string? Note { get; set; }

        public virtual Appointment? Appointment { get; set; }
        public virtual Staff? TaskExecutor { get; set; }
    }
}