
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
        [Column("maintenance_plan_id")]
        public Guid MaintenancePlanId { get; set; }

        [ForeignKey(nameof(MaintenancePlanId))]
        public MaintenancePlan? MaintenancePlan { get; set; }

        public virtual ICollection<Vehicle>? Vehicles { get; set; }
        
        public virtual ICollection<ModelPartType>? ModelPartTypes { get; set; }
    }
}