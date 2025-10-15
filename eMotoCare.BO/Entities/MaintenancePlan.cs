
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("maintenance_plan")]
    public class MaintenancePlan : BaseEntity
    {
        [Key]
        [Column("maintenance_plan_id")]
        public Guid Id { get; set; }
        public virtual ICollection<Model>? Models { get; set; }
        public virtual ICollection<MaintenanceStage>? MaintenanceStages { get; set; }
    }
}