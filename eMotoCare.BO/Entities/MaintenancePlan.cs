using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("maintenance_plan")]
    public class MaintenancePlan : BaseEntity
    {
        [Key]
        [Column("maintenance_plan_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; } = default!;

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("description")]
        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Column("unit", TypeName = "varchar(200)")]
        [EnumDataType(typeof(MaintenanceUnit))]
        public MaintenanceUnit Unit { get; set; }

        [Required]
        [Column("total_stages")]
        public int TotalStages { get; set; }

        [Required]
        [Column("effective_date")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        public virtual ICollection<Model>? Models { get; set; }
        public virtual ICollection<MaintenanceStage>? MaintenanceStages { get; set; }
    }
}
