

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("ec_check")]
    public class EVCheck
    {
        [Key]
        [Column("ev_check_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("appointment_id")]
        public Guid AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment? Appointment { get; set; }

        [Required]
        [Column("task_executor_id")]
        public Guid TaskExecutorId { get; set; }

        [ForeignKey(nameof(TaskExecutorId))]
        public virtual Staff? Staff { get; set; }
        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }
    }
}