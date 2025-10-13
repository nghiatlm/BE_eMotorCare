
using eMotoCare.Common.Enums;


namespace eMotoCare.DAL.Entities
{
    public class EVCheck : BaseEntity
    {
        public Guid EVCheckId { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public Guid TaskExecutorId { get; set; }
        public Staff? TaskExecutor { get; set; }
        public DateTime CheckDate { get; set; }
        public Status Status { get; set; }
        public string? Note { get; set; }

        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }


    }
}