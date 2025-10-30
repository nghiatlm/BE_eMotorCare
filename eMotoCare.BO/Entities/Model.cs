using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("model")]
    public class Model
    {
        [Key]
        [Column("model_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code", TypeName = "nvarchar(100)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("manufacturer", TypeName = "nvarchar(300)")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [Column("maintenance_plan_id")]
        public Guid MaintenancePlanId { get; set; }

        [ForeignKey(nameof(MaintenancePlanId))]
        public MaintenancePlan? MaintenancePlan { get; set; }

        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public virtual ICollection<Vehicle>? Vehicles { get; set; }

        public virtual ICollection<ModelPartType>? ModelPartTypes { get; set; }
    }
}
