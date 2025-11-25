using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("service_center_slot")]
    public class ServiceCenterSlot
    {
        [Key]
        [Column("service_center_slot_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }

        [Required]
        [Column("date")]
        public DateOnly Date { get; set; }

        [Required]
        [Column("day_of_week", TypeName = "varchar(20)")]
        [EnumDataType(typeof(DayOfWeeks))]
        public DayOfWeeks DayOfWeek { get; set; }

        [Required]
        [Column("slot_time", TypeName = "varchar(20)")]
        [EnumDataType(typeof(SlotTime))]
        public SlotTime SlotTime { get; set; }

        [Required]
        [Column("capacity")]
        public int Capacity { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("note")]
        public string? Note { get; set; }
    }
}
