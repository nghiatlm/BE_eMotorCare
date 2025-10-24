using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("ev_check")]
    public class EVCheck : BaseEntity
    {
        [Key]
        [Column("ev_check_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("check_date")]
        public DateTime CheckDate { get; set; }

        [Column("total_amout")]
        public decimal? TotalAmout { get; set; }

        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(EVCheckStatus))]
        public EVCheckStatus Status { get; set; }

        [Required]
        [Column("odometer")]
        public int Odometer { get; set; }
        [Required]
        [Column("appointment_id")]
        public Guid AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment? Appointment { get; set; }

        [Required]
        [Column("task_executor_id")]
        public Guid TaskExecutorId { get; set; }

        [ForeignKey(nameof(TaskExecutorId))]
        public virtual Staff? TaskExecutor { get; set; }
        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }
    }
}