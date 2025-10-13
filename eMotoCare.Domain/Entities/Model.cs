
namespace eMotoCare.DAL.Entities
{
    public class Model
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public Guid MaintenancePlanId { get; set; }
        public string? Description { get; set; }

        public MaintenancePlan? MaintenancePlan { get; set; }

        public virtual ICollection<ModelPartType>? ModelPartTypes { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; } 
    }
}